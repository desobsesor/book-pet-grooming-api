using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Session entity.
/// </summary>
public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor for SessionRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a session by its ID
    /// </summary>
    /// <param name="id">Session ID</param>
    /// <returns>Session if found, null otherwise</returns>
    public async Task<Session?> GetByIdAsync(int id)
    {
        return await _context.Sessions.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all sessions
    /// </summary>
    /// <returns>Collection of all sessions</returns>
    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        return await _context.Sessions.ToListAsync();
    }

    /// <summary>
    /// Retrieves a session by its token
    /// </summary>
    /// <param name="token">Session token</param>
    /// <returns>Session if found, null otherwise</returns>
    public async Task<Session?> GetByTokenAsync(string token)
    {
        return await _context.Sessions
            .FirstOrDefaultAsync(s => s.Token == token);
    }

    /// <summary>
    /// Retrieves all sessions for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Collection of sessions for the specified user</returns>
    public async Task<IEnumerable<Session>> GetByUserIdAsync(int userId)
    {
        return await _context.Sessions
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all active (non-expired) sessions
    /// </summary>
    /// <returns>Collection of active sessions</returns>
    public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
    {
        var currentTime = DateTime.UtcNow;
        return await _context.Sessions
            .Where(s => s.ExpiresAt > currentTime)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new session
    /// </summary>
    /// <param name="session">Session to add</param>
    /// <returns>ID of the newly created session</returns>
    public async Task<int> AddAsync(Session session)
    {
        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();
        return session.SessionId;
    }

    /// <summary>
    /// Updates an existing session
    /// </summary>
    /// <param name="session">Session to update</param>
    public async Task UpdateAsync(Session session)
    {
        _context.Sessions.Update(session);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a session by its ID
    /// </summary>
    /// <param name="id">Session ID</param>
    public async Task DeleteAsync(int id)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes all expired sessions
    /// </summary>
    public async Task DeleteExpiredSessionsAsync()
    {
        var currentTime = DateTime.UtcNow;
        var expiredSessions = await _context.Sessions
            .Where(s => s.ExpiresAt <= currentTime)
            .ToListAsync();

        if (expiredSessions.Any())
        {
            _context.Sessions.RemoveRange(expiredSessions);
            await _context.SaveChangesAsync();
        }
    }
}