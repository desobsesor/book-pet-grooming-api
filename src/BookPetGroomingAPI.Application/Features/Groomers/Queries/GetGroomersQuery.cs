using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    public class GetGroomersQuery : IRequest<List<GroomerDto>>
    {
    }
}