using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces;

/// <summary>
/// Repository interface for Groomer entity operations.
/// </summary>
public interface IGroomerRepository
{
    Task<Groomer?> GetByIdAsync(int id);
    Task<IEnumerable<Groomer>> GetAllAsync();
    Task<IEnumerable<Groomer>> GetActiveAsync();
    Task<int> AddAsync(Groomer groomer);
    Task UpdateAsync(Groomer groomer);
    Task DeleteAsync(int id);
    Task<bool> GroomerExistsAsync(string email);
}