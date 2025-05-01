using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    public class GetNotificationByIdQuery(int notificationId) : IRequest<NotificationDto>
    {
        public int NotificationId { get; } = notificationId;
    }
}