using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    public class DeleteNotificationCommand(int notificationId) : IRequest<int>
    {
        public int NotificationId { get; } = notificationId;
    }
}