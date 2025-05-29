using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BookPetGroomingAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookPetGroomingAPI.IntegrationTests.Fixtures;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;

    public ApiWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureAppConfiguration((context, config) =>
        {
            _ = config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "DataSource=:memory:"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            // Replace the DbContext with an in-memory one for testing
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            // Ensure the database is created and initialized with test data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory>>();

            try
            {
                db.Database.EnsureCreated();
                SeedTestData(db);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error initializing test database.");
            }
        });
    }

    private static void SeedTestData(ApplicationDbContext context)
    {
        // Add test data for the required entities
        // Example:
        /*
        context.Customers.AddRange(
            new Customer { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com", PhoneNumber = "1234567890" },
            new Customer { Id = 2, FirstName = "Another", LastName = "User", Email = "another@example.com", PhoneNumber = "0987654321" }
        );

        context.Groomers.AddRange(
            new Groomer { Id = 1, FirstName = "Test", LastName = "Groomer", Email = "groomer@example.com" }
        );
        */

        context.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}