using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces;

/// <summary>
/// Repository interface for Customer entity operations.
/// </summary>
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> GetActiveAsync();
    Task<int> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<bool> CustomerExistsAsync(string email);
}