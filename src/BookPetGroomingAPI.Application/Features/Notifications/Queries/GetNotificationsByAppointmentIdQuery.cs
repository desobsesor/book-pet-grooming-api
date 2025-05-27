using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Query to retrieve a list of notifications belonging to a specific appointment
    /// </summary>
    public class GetNotificationsByAppointmentIdQuery(int appointmentId) : IRequest<List<NotificationDto>>
    {
        /// <summary>
        /// The unique identifier of the appointment whose notifications are being requested 
        /// </summary>
        public int AppointmentId { get; } = appointmentId;
    }
}