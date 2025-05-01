using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    public class UpdateNotificationCommand(int notificationId, string title, string message) : IRequest<int>
    {
        public int NotificationId { get; set; } = notificationId;
        public string Title { get; set; } = title;
        public string Message { get; set; } = message;
    }
}