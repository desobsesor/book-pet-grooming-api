using Microsoft.EntityFrameworkCore;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Infrastructure.Persistence;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Groomer entity.
/// </summary>
public class GroomerRepository(ApplicationDbContext context) : IGroomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Groomer?> GetByIdAsync(int id)
    {
        return await _context.Groomers.FindAsync(id);
    }

    public async Task<IEnumerable<Groomer>> GetAllAsync()
    {
        return await _context.Groomers.ToListAsync();
    }

    public async Task<IEnumerable<Groomer>> GetActiveAsync()
    {
        return await _context.Groomers
            .Where(g => g.FirstName != null)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Groomer groomer)
    {
        _context.Groomers.Add(groomer);
        await _context.SaveChangesAsync();
        return groomer.GroomerId;
    }

    public async Task UpdateAsync(Groomer groomer)
    {
        _context.Groomers.Update(groomer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var groomer = await _context.Groomers.FindAsync(id);
        if (groomer != null)
        {
            //groomer.Deactivate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> GroomerExistsAsync(string email)
    {
        return await _context.Groomers
            .AnyAsync(g => g.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
    }
}