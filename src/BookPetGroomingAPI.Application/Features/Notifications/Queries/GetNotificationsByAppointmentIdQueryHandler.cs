using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Queries
{
    /// <summary>
    /// Handler for processing the GetNotificationsByAppointmentIdQuery and returning the list of notifications for an appointment
    /// </summary>
    public class GetNotificationsByAppointmentIdQueryHandler(INotificationRepository notificationRepository, IMapper mapper) : IRequestHandler<GetNotificationsByAppointmentIdQuery, List<NotificationDto>>
    {
        public async Task<List<NotificationDto>> Handle(GetNotificationsByAppointmentIdQuery request, CancellationToken cancellationToken)
        {
            var notifications = await notificationRepository.GetByAppointmentIdAsync(request.AppointmentId);
            return mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}