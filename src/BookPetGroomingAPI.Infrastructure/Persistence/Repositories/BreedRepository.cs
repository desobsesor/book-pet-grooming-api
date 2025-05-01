using Microsoft.EntityFrameworkCore;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Infrastructure.Persistence;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Breed entity.
/// </summary>
public class BreedRepository(ApplicationDbContext context) : IBreedRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Breed>> GetAllAsync()
    {
        return await _context.Breeds.ToListAsync();
    }

    public async Task<Breed> GetByIdAsync(int id)
    {
        return await _context.Breeds.FindAsync(id);
    }

    public async Task AddAsync(Breed breed)
    {
        _context.Breeds.Add(breed);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Breed breed)
    {
        _context.Breeds.Update(breed);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var breed = await _context.Breeds.FindAsync(id);
        if (breed != null)
        {
            _context.Breeds.Remove(breed);
            await _context.SaveChangesAsync();
        }
    }
}