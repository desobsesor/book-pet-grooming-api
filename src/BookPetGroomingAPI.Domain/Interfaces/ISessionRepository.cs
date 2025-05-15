using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces;

/// <summary>
/// Repository interface for Session entity operations.
/// </summary>
public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(int id);
    Task<IEnumerable<Session>> GetAllAsync();
    Task<Session?> GetByTokenAsync(string token);
    Task<IEnumerable<Session>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Session>> GetActiveSessionsAsync();
    Task<int> AddAsync(Session session);
    Task UpdateAsync(Session session);
    Task DeleteAsync(int id);
    Task DeleteExpiredSessionsAsync();
}