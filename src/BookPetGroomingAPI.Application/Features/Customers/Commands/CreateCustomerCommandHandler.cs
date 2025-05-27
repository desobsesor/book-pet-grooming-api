using AutoMapper;
using BookPetGroomingAPI.Application.Common;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Commands
{
    /// <summary>
    /// Handler for processing the CreateCustomerCommand and creating a new customer
    /// </summary>
    /// <remarks>
    /// Constructor for CreateCustomerCommandHandler
    /// </remarks>
    /// <param name="customerRepository">Repository for customer operations</param>
    /// <param name="mapper">Mapping service</param>
    public class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateCustomerCommand, int>
    {

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
                var username = SecurityUtils.GenerateUniqueUsername(request.FirstName, request.LastName);
                var existingCustomer = await userRepository.UserExistsAsync(username);

                if (existingCustomer)
                {
                    username = SecurityUtils.GenerateUniqueUsername(request.FirstName, request.Email.Split('@')[0]);
                }
                // Create a new user entity for the customer
                var user = new User(
                    username: username,
                    email: request.Email,
                    passwordHash: SecurityUtils.GenerateSecurePassword(),
                    role: "customer");

                // Add the user to the repository
                var userId = await userRepository.AddAsync(user);

                // Create a new customer entity using the domain constructor
                // This approach respects the domain model's encapsulation and validation
                var customer = new Customer(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: request.Email,
                    phone: request.Phone,
                    address: request.Address,
                    preferredGroomerId: null,
                    userId: userId); // Assign the created user ID to the customer

                // Add the customer to the repository
                var customerId = await customerRepository.AddAsync(customer);

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