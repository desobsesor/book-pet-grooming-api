using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Customer entity.
/// </summary>
public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers.FindAsync(customerId);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetActiveAsync()
    {
        return await _context.Customers
            .Where(c => c.FirstName != null)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer.CustomerId;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            //customer.Deactivate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CustomerExistsAsync(string email)
    {
        return await _context.Customers
            .AnyAsync(c => c.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
    }
}