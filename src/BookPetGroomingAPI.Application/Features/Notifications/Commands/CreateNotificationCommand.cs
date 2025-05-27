using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand(string recipientType, string message, int isRead, int? appointmentId) : IRequest<int>
    {
        public string RecipientType { get; set; } = recipientType;
        public string Message { get; set; } = message;
        public int IsRead { get; set; } = isRead;
        public int? AppointmentId { get; set; } = appointmentId;
    }
}