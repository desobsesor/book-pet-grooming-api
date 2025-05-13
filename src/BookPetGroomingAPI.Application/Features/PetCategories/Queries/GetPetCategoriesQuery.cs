using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Queries
{
    public class GetPetCategoriesQuery : IRequest<List<PetCategoryDto>>
    {
    }
}