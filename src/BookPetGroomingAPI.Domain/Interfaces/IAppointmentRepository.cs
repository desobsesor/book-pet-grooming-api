using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces;

/// <summary>
/// Repository interface for Appointment entity operations.
/// </summary>
public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id);
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<IEnumerable<Appointment>> GetActiveAsync();
    Task<IEnumerable<Appointment>> GetByGroomerIdAsync(int groomerId);
    Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId);
    Task<IEnumerable<Appointment>> GetByAppointmentDateAsync(DateTime appointmentDate);
    Task<IEnumerable<Appointment>> GetByStatusAsync(string status);
    Task<int> AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(int id);
    Task<bool> AppointmentExistsAsync(int petId, DateTime appointmentDate);
}