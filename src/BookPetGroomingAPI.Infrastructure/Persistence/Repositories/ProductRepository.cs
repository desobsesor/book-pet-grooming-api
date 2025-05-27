using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveAsync()
    {
        return await context.Products
            .Where(p => p.Active)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product.Id;
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            product.Deactivate();
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ProductExistsAsync(string name)
    {
        return await context.Products
            .AnyAsync(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
}