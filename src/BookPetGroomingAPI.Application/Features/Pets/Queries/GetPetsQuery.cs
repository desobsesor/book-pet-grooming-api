using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    public class GetPetsQuery : IRequest<List<PetDto>>
    {
    }
}