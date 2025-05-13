using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Commands
{
    /// <summary>
    /// Handler for processing the CreateCustomerCommand and creating a new customer
    /// </summary>
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for CreateCustomerCommandHandler
        /// </summary>
        /// <param name="customerRepository">Repository for customer operations</param>
        /// <param name="mapper">Mapping service</param>
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to create a new customer
        /// </summary>
        /// <param name="request">The CreateCustomerCommand with customer data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created customer</returns>
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new ArgumentException("First name is required", nameof(request.FirstName));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new customer entity using the domain constructor
                // This approach respects the domain model's encapsulation and validation
                var customer = new Customer(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: request.Email,
                    phone: request.Phone,
                    address: request.Address,
                    preferredGroomerId: null);

                // Add the customer to the repository
                var customerId = await _customerRepository.AddAsync(customer);

                return customerId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping customer data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating customer", ex);
            }
        }
    }
}