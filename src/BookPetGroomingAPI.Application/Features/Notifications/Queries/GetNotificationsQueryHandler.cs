using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Handler for processing the GetNotificationsQuery and returning the list of notifications
    /// </summary>
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetNotificationsQueryHandler
        /// </summary>
        /// <param name="notificationRepository">Repository for notification operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all notifications
        /// </summary>
        /// <param name="request">The GetNotificationsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of notification DTOs</returns>
        public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return _mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}