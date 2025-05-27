using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Session entity.
/// </summary>
/// <remarks>
/// Constructor for SessionRepository
/// </remarks>
/// <param name="context">Database context</param>
public class SessionRepository(ApplicationDbContext context) : ISessionRepository
{

    /// <summary>
    /// Retrieves a session by its ID
    /// </summary>
    /// <param name="id">Session ID</param>
    /// <returns>Session if found, null otherwise</returns>
    public async Task<Session?> GetByIdAsync(int id)
    {
        return await context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionId == id);
    }

    /// <summary>
    /// Retrieves all sessions
    /// </summary>
    /// <returns>Collection of all sessions</returns>
    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        return await context.Sessions
        .Include(s => s.User)
        .ToListAsync();
    }

    /// <summary>
    /// Retrieves a session by its token
    /// </summary>
    /// <param name="token">Session token</param>
    /// <returns>Session if found, null otherwise</returns>
    public async Task<Session?> GetByTokenAsync(string token)
    {
        return await context.Sessions
        .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token);
    }

    /// <summary>
    /// Retrieves all sessions for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Collection of sessions for the specified user</returns>
    public async Task<IEnumerable<Session>> GetByUserIdAsync(int userId)
    {
        return await context.Sessions
            .Include(s => s.User)
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
        return await context.Sessions
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
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync();
        return session.SessionId;
    }

    /// <summary>
    /// Updates an existing session
    /// </summary>
    /// <param name="session">Session to update</param>
    public async Task UpdateAsync(Session session)
    {
        context.Sessions.Update(session);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a session by its ID
    /// </summary>
    /// <param name="id">Session ID</param>
    public async Task DeleteAsync(int id)
    {
        var session = await context.Sessions.FindAsync(id);
        if (session != null)
        {
            context.Sessions.Remove(session);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes all expired sessions
    /// </summary>
    public async Task DeleteExpiredSessionsAsync()
    {
        var currentTime = DateTime.UtcNow;
        var expiredSessions = await context.Sessions
            .Where(s => s.ExpiresAt <= currentTime)
            .ToListAsync();

        if (expiredSessions.Any())
        {
            context.Sessions.RemoveRange(expiredSessions);
            await context.SaveChangesAsync();
        }
    }
}