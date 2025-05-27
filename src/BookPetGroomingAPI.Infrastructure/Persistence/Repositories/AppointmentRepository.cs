using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Appointment entity.
/// </summary>
public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetActiveAsync()
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .Where(a => a.Status == "Scheduled" || a.Status == "Pending")
            .ToListAsync();
    }

    public async Task<int> AddAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment.AppointmentId;
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AppointmentExistsAsync(int petId, DateTime appointmentDate)
    {
        return await _context.Appointments
            .AnyAsync(a => a.PetId == petId && a.AppointmentDate == appointmentDate);
    }

    public async Task<IEnumerable<Appointment>> GetByGroomerIdAsync(int groomerId)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .Where(a => a.GroomerId == groomerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .Where(a => a.PetId == petId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByAppointmentDateAsync(DateTime appointmentDate)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .Where(a => a.AppointmentDate == appointmentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Groomer)
            .Where(a => a.Status == status)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.Include(a => a.Notifications).FirstOrDefaultAsync(a => a.AppointmentId == id);
        if (appointment != null)
        {
            _context.Notifications.RemoveRange(appointment.Notifications);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}