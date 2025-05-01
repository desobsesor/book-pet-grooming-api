using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery(int id) : IRequest<CustomerDto>
    {
        public int Id { get; } = id;
    }
}