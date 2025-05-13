using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Commands
{
    /// <summary>
    /// Command to create a new pet grooming appointment
    /// </summary>
    public class CreateAppointmentCommand(int petId, int groomerId, DateTime appointmentDate, TimeOnly startTime, TimeSpan estimatedDuration, string status, decimal price, string? notes) : IRequest<int>
    {
        /// <summary>
        /// Pet ID for the appointment
        /// </summary>
        public int PetId { get; set; } = petId;

        /// <summary>
        /// Assigned groomer ID
        /// </summary>
        public int GroomerId { get; set; } = groomerId;

        /// <summary>
        /// Appointment date
        /// </summary>
        public DateTime AppointmentDate { get; set; } = appointmentDate;

        /// <summary>
        /// Start time of the appointment
        /// </summary>
        public TimeOnly StartTime { get; set; } = startTime;

        /// <summary>
        /// Estimated duration of the appointment
        /// </summary>
        public TimeSpan EstimatedDuration { get; set; } = estimatedDuration;

        /// <summary>
        /// Current status of the appointment (e.g. Scheduled, Completed, Cancelled)
        /// </summary>
        public required string Status { get; set; } = status;

        /// <summary>
        /// Service price
        /// </summary>
        public decimal Price { get; set; } = price;

        /// <summary>
        /// Additional notes about the appointment
        /// </summary>
        public string? Notes { get; set; } = notes;
    }
}