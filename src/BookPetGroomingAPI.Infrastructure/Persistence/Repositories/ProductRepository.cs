using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveAsync()
    {
        return await _context.Products
            .Where(p => p.Active)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            product.Deactivate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ProductExistsAsync(string name)
    {
        return await _context.Products
            .AnyAsync(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
}