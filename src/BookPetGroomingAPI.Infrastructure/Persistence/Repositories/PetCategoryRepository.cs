using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for PetCategory entity.
/// </summary>
public class PetCategoryRepository(ApplicationDbContext context) : IPetCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<PetCategory>> GetAllAsync()
    {
        return await _context.PetCategories.ToListAsync();
    }

    public async Task<PetCategory> GetByIdAsync(int id)
    {
        return await _context.PetCategories.FindAsync(id);
    }

    public async Task AddAsync(PetCategory petCategory)
    {
        _context.PetCategories.Add(petCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PetCategory petCategory)
    {
        _context.PetCategories.Update(petCategory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var petCategory = await _context.PetCategories.FindAsync(id);
        if (petCategory != null)
        {
            _context.PetCategories.Remove(petCategory);
            await _context.SaveChangesAsync();
        }
    }
}