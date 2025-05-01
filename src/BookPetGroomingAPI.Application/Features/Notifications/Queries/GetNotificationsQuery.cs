using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
    }
}