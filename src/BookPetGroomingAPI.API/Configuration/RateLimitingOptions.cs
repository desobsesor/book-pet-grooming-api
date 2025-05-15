namespace BookPetGroomingAPI.API.Configuration;

/// <summary>
/// Configuration options for the rate limiting middleware
/// </summary>
public class RateLimitingOptions
{
    /// <summary>
    /// Gets or sets the general rate limit window in seconds
    /// </summary>
    public int WindowInSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the maximum number of requests allowed within the window
    /// </summary>
    public int MaxRequests { get; set; } = 100;

    /// <summary>
    /// Gets or sets whether to enable per-client IP rate limiting
    /// </summary>
    public bool EnableIpRateLimiting { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to enable per-user rate limiting (requires authentication)
    /// </summary>
    public bool EnableUserRateLimiting { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of requests allowed for authenticated users
    /// </summary>
    public int AuthenticatedMaxRequests { get; set; } = 200;

    /// <summary>
    /// Gets or sets the list of IP addresses that should be excluded from rate limiting
    /// </summary>
    public List<string> IpWhitelist { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of endpoints that should be excluded from rate limiting
    /// </summary>
    public List<string> EndpointWhitelist { get; set; } = new List<string>();
}