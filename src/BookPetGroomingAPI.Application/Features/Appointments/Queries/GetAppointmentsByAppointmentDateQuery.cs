using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Query to retrieve a list of appointments belonging to a specific AppointmentDate
    /// </summary>
    public class GetAppointmentsByAppointmentDateQuery(DateTime appointmentDate) : IRequest<List<AppointmentDto>>
    {
        /// <summary>
        /// The appointment date for which appointments are being requested 
        /// </summary>
        public DateTime AppointmentDate { get; } = appointmentDate;
    }
}