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
        return await _context.Appointments.FindAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetActiveAsync()
    {
        return await _context.Appointments
            .Where(a => a.Status != null)
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

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            //appointment.Deactivate();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> AppointmentExistsAsync(int petId, DateTime appointmentDate)
    {
        return await _context.Appointments
            .AnyAsync(a => a.PetId == petId && a.AppointmentDate == appointmentDate);
    }

    public async Task<IEnumerable<Appointment>> GetByGroomerIdAsync(int groomerId)
    {
        return await _context.Appointments
            .Where(p => p.GroomerId == groomerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId)
    {
        return await _context.Appointments
            .Where(p => p.PetId == petId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByAppointmentDateAsync()
    {
        return await _context.Appointments
            .Where(p => p.AppointmentDate == DateTime.UtcNow)  
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
    {
        return await _context.Appointments
            .Where(p => p.Status == status)
            .ToListAsync();
    }
}