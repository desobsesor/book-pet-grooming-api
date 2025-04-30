using Microsoft.EntityFrameworkCore;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Infrastructure.Persistence;

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
}