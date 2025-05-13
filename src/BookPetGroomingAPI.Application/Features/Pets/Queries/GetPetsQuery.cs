using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    public class GetPetsQuery : IRequest<List<PetDto>>
    {
    }
}