using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries
{
    public class GetBreedsQuery : IRequest<List<BreedDto>>
    {
    }
}