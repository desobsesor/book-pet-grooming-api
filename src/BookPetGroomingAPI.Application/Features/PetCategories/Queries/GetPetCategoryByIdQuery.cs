using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Queries
{
    public class GetPetCategoryByIdQuery(int id) : IRequest<PetCategoryDto>
    {
        public int Id { get; } = id;
    }
}