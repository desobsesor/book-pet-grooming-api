using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}