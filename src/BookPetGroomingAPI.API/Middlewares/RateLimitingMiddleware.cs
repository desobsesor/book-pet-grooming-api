using BookPetGroomingAPI.API.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace BookPetGroomingAPI.API.Middlewares;

/// <summary>
/// Middleware for implementing rate limiting to protect API endpoints
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly IMemoryCache _cache;
    private readonly RateLimitingOptions _options;
    private readonly ConcurrentDictionary<string, DateTime> _lastErrorNotificationTimes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitingMiddleware"/> class
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">The logger</param>
    /// <param name="cache">The memory cache for storing rate limit counters</param>
    /// <param name="options">The rate limiting configuration options</param>
    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger,
        IMemoryCache cache, IOptions<RateLimitingOptions> options)
    {
        _next = next;
        _logger = logger;
        _cache = cache;
        _options = options.Value;
    }

    /// <summary>
    /// Processes the request and applies rate limiting rules
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip rate limiting for whitelisted endpoints
        var endpoint = context.GetEndpoint()?.DisplayName;
        if (endpoint != null && _options.EndpointWhitelist.Any(e => endpoint.Contains(e, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Skip rate limiting for whitelisted IPs
        var clientIp = GetClientIpAddress(context);
        if (_options.IpWhitelist.Contains(clientIp))
        {
            await _next(context);
            return;
        }

        // Determine the client identifier based on configuration
        string clientId = GetClientIdentifier(context, clientIp);

        // Get the appropriate rate limit based on authentication status
        int maxRequests = context.User.Identity?.IsAuthenticated == true
            ? _options.AuthenticatedMaxRequests
            : _options.MaxRequests;

        // Check if the client has exceeded the rate limit
        if (IsRateLimitExceeded(clientId, maxRequests))
        {
            await HandleRateLimitExceeded(context, clientId);
            return;
        }

        // Add rate limit headers to the response
        AddRateLimitHeaders(context, clientId, maxRequests);

        // Continue with the next middleware in the pipeline
        await _next(context);
    }

    private string GetClientIdentifier(HttpContext context, string clientIp)
    {
        // Use user ID for authenticated users if user rate limiting is enabled
        if (_options.EnableUserRateLimiting && context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                return $"user_{userId}";
            }
        }

        // Fall back to IP-based rate limiting if enabled
        if (_options.EnableIpRateLimiting)
        {
            return $"ip_{clientIp}";
        }

        // Default to a global rate limit if neither is enabled
        return "global";
    }

    private string GetClientIpAddress(HttpContext context)
    {
        // Try to get the client IP from various headers or connection info
        string? clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(clientIp))
        {
            clientIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(clientIp))
        {
            clientIp = context.Connection.RemoteIpAddress?.ToString();
        }

        return clientIp ?? "unknown";
    }

    private bool IsRateLimitExceeded(string clientId, int maxRequests)
    {
        var cacheKey = $"ratelimit_{clientId}";

        // Get or create a counter for the client
        if (!_cache.TryGetValue(cacheKey, out SlidingWindowCounter counter))
        {
            counter = new SlidingWindowCounter(_options.WindowInSeconds);
            _cache.Set(cacheKey, counter, TimeSpan.FromSeconds(_options.WindowInSeconds * 2));
        }

        // Increment the counter and check if the limit is exceeded
        return counter.Increment() > maxRequests;
    }

    private async Task HandleRateLimitExceeded(HttpContext context, string clientId)
    {
        // Set the response status code to 429 (Too Many Requests)
        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
        context.Response.ContentType = "application/json";

        // Add retry-after header
        context.Response.Headers.Append("Retry-After", _options.WindowInSeconds.ToString());

        // Log the rate limit exceeded event (with throttling to prevent log flooding)
        LogRateLimitExceeded(clientId);

        // Return a JSON response with information about the rate limit
        var response = new
        {
            error = "Too many requests",
            retryAfter = _options.WindowInSeconds,
            message = $"Rate limit exceeded. Try again in {_options.WindowInSeconds} seconds."
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private void LogRateLimitExceeded(string clientId)
    {
        // Throttle logging to prevent log flooding
        var now = DateTime.UtcNow;
        var lastLogTime = _lastErrorNotificationTimes.GetOrAdd(clientId, now.AddMinutes(-10));

        if ((now - lastLogTime).TotalMinutes >= 1)
        {
            _logger.LogWarning("Rate limit exceeded for client {ClientId}", clientId);
            _lastErrorNotificationTimes[clientId] = now;
        }
    }

    private void AddRateLimitHeaders(HttpContext context, string clientId, int maxRequests)
    {
        var cacheKey = $"ratelimit_{clientId}";
        if (_cache.TryGetValue(cacheKey, out SlidingWindowCounter counter))
        {
            int remaining = Math.Max(0, maxRequests - counter.Count);

            // Add standard rate limit headers
            context.Response.Headers.Append("X-RateLimit-Limit", maxRequests.ToString());
            context.Response.Headers.Append("X-RateLimit-Remaining", remaining.ToString());
            context.Response.Headers.Append("X-RateLimit-Reset", counter.WindowResetTime.ToString("o"));
        }
    }

    /// <summary>
    /// Helper class for implementing a sliding window counter
    /// </summary>
    private class SlidingWindowCounter
    {
        private readonly int _windowSeconds;
        private readonly Queue<DateTime> _requests = new();
        private readonly object _lock = new();

        public int Count => _requests.Count;
        public DateTime WindowResetTime { get; private set; }

        public SlidingWindowCounter(int windowSeconds)
        {
            _windowSeconds = windowSeconds;
            WindowResetTime = DateTime.UtcNow.AddSeconds(windowSeconds);
        }

        public int Increment()
        {
            lock (_lock)
            {
                var now = DateTime.UtcNow;
                var windowStart = now.AddSeconds(-_windowSeconds);

                // Remove requests outside the current window
                while (_requests.Count > 0 && _requests.Peek() < windowStart)
                {
                    _requests.Dequeue();
                }

                // Add the current request
                _requests.Enqueue(now);

                // Update the window reset time
                WindowResetTime = now.AddSeconds(_windowSeconds);

                return _requests.Count;
            }
        }
    }
}