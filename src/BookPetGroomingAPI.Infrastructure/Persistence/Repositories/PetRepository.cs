using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Pet entity.
/// </summary>
public class PetRepository(ApplicationDbContext context) : IPetRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Pet?> GetByIdAsync(int id)
    {
        return await _context.Pets.FindAsync(id);
    }

    public async Task<IEnumerable<Pet>> GetAllAsync()
    {
        return await _context.Pets.ToListAsync();
    }

    public async Task<IEnumerable<Pet>> GetActiveAsync()
    {
        return await _context.Pets
            .Where(p => p.Name != null)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Pet pet)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
        return pet.PetId;
    }

    public async Task UpdateAsync(Pet pet)
    {
        _context.Pets.Update(pet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet != null)
        {
            // pet.Deactivate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> PetExistsAsync(string name, int customerId)
    {
        return await _context.Pets
            .AnyAsync(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && p.CustomerId == customerId);
    }

    public async Task<IEnumerable<Pet>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Pets
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();
    }

}