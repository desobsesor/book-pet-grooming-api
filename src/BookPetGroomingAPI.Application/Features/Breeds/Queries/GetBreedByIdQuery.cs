using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries
{
    public class GetBreedByIdQuery(int breedId) : IRequest<BreedDto>
    {
        public int BreedId { get; } = breedId;
    }
}