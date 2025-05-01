using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Queries
{
    public class GetPetCategoriesQuery : IRequest<List<PetCategoryDto>>
    {
    }
}