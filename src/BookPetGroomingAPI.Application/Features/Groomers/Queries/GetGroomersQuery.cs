using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    public class GetGroomersQuery : IRequest<List<GroomerDto>>
    {
    }
}