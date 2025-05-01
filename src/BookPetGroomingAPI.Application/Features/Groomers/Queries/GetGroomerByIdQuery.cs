using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    public class GetGroomerByIdQuery(int groomerId) : IRequest<GroomerDto>
    {
        public int GroomerId { get; } = groomerId;
    }
}