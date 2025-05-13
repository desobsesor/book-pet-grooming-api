using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    public class GetPetByIdQuery(int petId) : IRequest<PetDto>
    {
        public int PetId { get; } = petId;
    }
}