using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    public class GetPetByIdQuery(int id) : IRequest<PetDto>
    {
        public int Id { get; } = id;
    }
}