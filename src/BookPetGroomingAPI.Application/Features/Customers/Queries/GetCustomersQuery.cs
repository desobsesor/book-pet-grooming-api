using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}