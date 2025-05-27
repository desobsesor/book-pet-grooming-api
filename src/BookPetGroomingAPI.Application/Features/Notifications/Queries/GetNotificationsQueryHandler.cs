using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Handler for processing the GetNotificationsQuery and returning the list of notifications
    /// </summary>
    /// <remarks>
    /// Constructor for GetNotificationsQueryHandler
    /// </remarks>
    /// <param name="notificationRepository">Repository for notification operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetNotificationsQueryHandler(INotificationRepository notificationRepository, IMapper mapper) : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all notifications
        /// </summary>
        /// <param name="request">The GetNotificationsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of notification DTOs</returns>
        public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await notificationRepository.GetAllAsync();
            return mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}