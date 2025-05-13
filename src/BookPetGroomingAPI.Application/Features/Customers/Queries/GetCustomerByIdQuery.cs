using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery(int customerId) : IRequest<CustomerDto>
    {
        public int CustomerId { get; } = customerId;
    }
}