using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces;

/// <summary>
/// Repository interface for Pet entity operations.
/// </summary>
public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(int id);
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<IEnumerable<Pet>> GetActiveAsync();
    Task<int> AddAsync(Pet pet);
    Task UpdateAsync(Pet pet);
    Task DeleteAsync(int id);
    Task<bool> PetExistsAsync(string name, int customerId);
}