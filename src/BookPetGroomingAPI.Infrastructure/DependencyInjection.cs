using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Infrastructure.Persistence;
using BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

namespace BookPetGroomingAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IGroomerRepository, GroomerRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPetCategoryRepository, PetCategoryRepository>();
        services.AddScoped<IPetRepository, PetRepository>();

        return services;
    }
}