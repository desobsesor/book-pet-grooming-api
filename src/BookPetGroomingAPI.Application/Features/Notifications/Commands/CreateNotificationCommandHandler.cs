using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    /// <summary>
    /// Handler for processing the CreateNotificationCommand and creating a new notification
    /// </summary>
    public class CreateNotificationCommandHandler(INotificationRepository notificationRepository, IMapper mapper) : IRequestHandler<CreateNotificationCommand, int>
    {

        /// <summary>
        /// Handles the command to create a new notification
        /// </summary>
        /// <param name="request">The CreateNotificationCommand with notification data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created notification</returns>
        public async Task<int> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("Message is required", nameof(request.Message));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new notification entity
                Notification notification = new(
                    request.AppointmentId,
                    request.Message,
                    request.RecipientType,
                    request.IsRead
                );

                // Add the notification to the repository
                var notificationId = await notificationRepository.AddAsync(notification);

                return notificationId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping notification data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating notification", ex);
            }
        }
    }
}