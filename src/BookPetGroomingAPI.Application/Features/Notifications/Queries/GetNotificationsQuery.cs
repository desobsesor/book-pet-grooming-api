using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
    }
}