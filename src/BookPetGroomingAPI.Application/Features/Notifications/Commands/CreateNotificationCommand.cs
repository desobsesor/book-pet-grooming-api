using MediatR;

namespace BookPetGroomingAPI.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand(string title, string message) : IRequest<int>
    {
        public string Title { get; set; } = title;
        public string Message { get; set; } = message;
    }
}