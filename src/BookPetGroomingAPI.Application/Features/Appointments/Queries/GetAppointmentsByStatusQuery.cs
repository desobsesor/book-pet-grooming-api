using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Query to retrieve a list of appointments belonging to a specific status
    /// </summary>
    public class GetAppointmentsByStatusQuery(string status) : IRequest<List<AppointmentDto>>
    {
        /// <summary>
        /// The status for which appointments are being requested 
        /// </summary>
        public string Status { get; } = status;
    }
}