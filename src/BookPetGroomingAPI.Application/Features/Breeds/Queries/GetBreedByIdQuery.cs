using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries
{
    public class GetBreedByIdQuery(int id) : IRequest<BreedDto>
    {
        public int Id { get; } = id;
    }
}