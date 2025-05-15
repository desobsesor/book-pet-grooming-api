using BookPetGroomingAPI.API.Configuration;
using BookPetGroomingAPI.API.Middlewares;
using Microsoft.Extensions.Options;

namespace BookPetGroomingAPI.API.Extensions;

/// <summary>
/// Extension methods for configuring rate limiting services
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Adds rate limiting services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure rate limiting options from appsettings.json
        services.Configure<RateLimitingOptions>(configuration.GetSection("RateLimiting"));

        // Add memory cache for storing rate limit counters
        services.AddMemoryCache();

        return services;
    }

    /// <summary>
    /// Adds the rate limiting middleware to the application pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
    {
        // Get the options to check if rate limiting is enabled
        var options = app.ApplicationServices.GetRequiredService<IOptions<RateLimitingOptions>>().Value;

        // Apply the rate limiting middleware
        app.UseMiddleware<RateLimitingMiddleware>();

        return app;
    }
}