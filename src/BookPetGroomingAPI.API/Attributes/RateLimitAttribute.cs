using BookPetGroomingAPI.API.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace BookPetGroomingAPI.API.Attributes;

/// <summary>
/// Attribute to apply specific rate limiting rules to controllers or actions
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RateLimitAttribute : ActionFilterAttribute
{
    private readonly int _maxRequests;
    private readonly int _windowInSeconds;
    private readonly bool _applyGlobalLimits;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitAttribute"/> class
    /// </summary>
    /// <param name="maxRequests">Maximum number of requests allowed in the time window</param>
    /// <param name="windowInSeconds">Time window in seconds</param>
    /// <param name="applyGlobalLimits">Whether to also apply global rate limits defined in configuration</param>
    public RateLimitAttribute(int maxRequests, int windowInSeconds = 60, bool applyGlobalLimits = true)
    {
        _maxRequests = maxRequests;
        _windowInSeconds = windowInSeconds;
        _applyGlobalLimits = applyGlobalLimits;
    }

    /// <summary>
    /// Executes before the action method is invoked
    /// </summary>
    /// <param name="context">The action executing context</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Get required services
        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<RateLimitingOptions>>().Value;

        // Skip if rate limiting is disabled or endpoint is whitelisted
        var endpoint = context.ActionDescriptor.DisplayName;
        if (endpoint != null && options.EndpointWhitelist.Any(e => endpoint.Contains(e, StringComparison.OrdinalIgnoreCase)))
        {
            base.OnActionExecuting(context);
            return;
        }

        // Get client identifier (IP or user ID)
        string clientId = GetClientIdentifier(context.HttpContext, options);

        // Check if rate limit is exceeded
        var cacheKey = $"ratelimit_attr_{clientId}_{context.ActionDescriptor.Id}";
        if (!cache.TryGetValue(cacheKey, out SlidingWindowCounter counter))
        {
            counter = new SlidingWindowCounter(_windowInSeconds);
            cache.Set(cacheKey, counter, TimeSpan.FromSeconds(_windowInSeconds * 2));
        }

        // Increment counter and check if limit exceeded
        if (counter.Increment() > _maxRequests)
        {
            // Set response for rate limit exceeded
            context.Result = new Microsoft.AspNetCore.Mvc.ContentResult
            {
                StatusCode = (int)HttpStatusCode.TooManyRequests,
                ContentType = "application/json",
                Content = System.Text.Json.JsonSerializer.Serialize(new
                {
                    error = "Too many requests",
                    retryAfter = _windowInSeconds,
                    message = $"Rate limit exceeded. Try again in {_windowInSeconds} seconds."
                })
            };

            // Add headers
            context.HttpContext.Response.Headers.Append("Retry-After", _windowInSeconds.ToString());
            context.HttpContext.Response.Headers.Append("X-RateLimit-Limit", _maxRequests.ToString());
            context.HttpContext.Response.Headers.Append("X-RateLimit-Remaining", "0");
            context.HttpContext.Response.Headers.Append("X-RateLimit-Reset", counter.WindowResetTime.ToString("o"));

            return;
        }

        // Add rate limit headers
        int remaining = Math.Max(0, _maxRequests - counter.Count);
        context.HttpContext.Response.Headers.Append("X-RateLimit-Limit", _maxRequests.ToString());
        context.HttpContext.Response.Headers.Append("X-RateLimit-Remaining", remaining.ToString());
        context.HttpContext.Response.Headers.Append("X-RateLimit-Reset", counter.WindowResetTime.ToString("o"));

        base.OnActionExecuting(context);
    }

    private string GetClientIdentifier(HttpContext context, RateLimitingOptions options)
    {
        // Use user ID for authenticated users if user rate limiting is enabled
        if (options.EnableUserRateLimiting && context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId.ToString()))
            {
                return $"user_{userId}";
            }
        }

        // Fall back to IP-based rate limiting
        string? clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(clientIp))
        {
            clientIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(clientIp))
        {
            clientIp = context.Connection.RemoteIpAddress?.ToString();
        }

        return $"ip_{clientIp ?? "unknown"}";
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