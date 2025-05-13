using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    /// <summary>
    /// Handler for processing the GetCustomerByIdQuery and retrieving a customer by ID
    /// </summary>
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetCustomerByIdQueryHandler
        /// </summary>
        /// <param name="customerRepository">Repository for customer operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve a customer by ID
        /// </summary>
        /// <param name="request">The GetCustomerByIdQuery with customer ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer DTO if found</returns>
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}