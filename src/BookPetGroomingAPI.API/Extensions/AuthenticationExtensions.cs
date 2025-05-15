using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookPetGroomingAPI.API.Extensions
{
    /// <summary>
    /// Extensions to configure JWT authentication and authorization
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Configures JWT authentication for the application
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <returns>Updated service collection</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment12345678901234");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Set to true in production
                    ValidateAudience = false, // Set to true in production
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Configure role-based authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
                options.AddPolicy("GroomerOnly", policy => policy.RequireRole("groomer"));
                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("customer"));
                options.AddPolicy("AdminOrGroomer", policy => policy.RequireRole("admin", "groomer"));
            });

            return services;
        }
    }
}