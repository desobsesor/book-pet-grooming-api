using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Handler for processing the GetNotificationByIdQuery and retrieving a notification by ID
    /// </summary>
    public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetNotificationByIdQueryHandler
        /// </summary>
        /// <param name="notificationRepository">Repository for notification operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve a notification by ID
        /// </summary>
        /// <param name="request">The GetNotificationByIdQuery with notification ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The notification DTO if found</returns>
        public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId) ?? throw new KeyNotFoundException($"Notification with ID {request.NotificationId} not found");
            return _mapper.Map<NotificationDto>(notification);
        }
    }
}