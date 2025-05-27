using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    /// <summary>
    /// Handler to process the Delete Notification Command and delete a notification by ID
    /// </summary>
    public class DeleteNotificationCommandHandler(INotificationRepository notificationRepository)
        : IRequestHandler<DeleteNotificationCommand, int>
    {
        public async Task<int> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            await notificationRepository.DeleteAsync(request.NotificationId);
            return request.NotificationId;
        }
    }
}