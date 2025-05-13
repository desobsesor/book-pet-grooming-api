using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Queries
{
    /// <summary>
    /// Handler for processing the GetCustomersQuery and returning the list of customers
    /// </summary>
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetCustomersQueryHandler
        /// </summary>
        /// <param name="customerRepository">Repository for customer operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all customers
        /// </summary>
        /// <param name="request">The GetCustomersQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of customer DTOs</returns>
        public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }
    }
}