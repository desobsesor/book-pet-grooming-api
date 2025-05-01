using Microsoft.EntityFrameworkCore;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Infrastructure.Persistence;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Notification entity.
/// </summary>
public class NotificationRepository(ApplicationDbContext context) : INotificationRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await _context.Notifications.ToListAsync();
    }

    public async Task<Notification> GetByIdAsync(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}