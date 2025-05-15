using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Commands
{
    /// <summary>
    /// Handler for processing the CreateUserCommand and creating a new user
    /// </summary>
    /// <remarks>
    /// Constructor for CreateUserCommandHandler
    /// </remarks>
    /// <param name="userRepository">Repository for user operations</param>
    /// <param name="mapper">Mapping service</param>
    public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, int>
    {

        /// <summary>
        /// Handles the command to create a new user
        /// </summary>
        /// <param name="request">The CreateUserCommand with user data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created user</returns>
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ArgumentException("Username is required", nameof(request.Username));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required", nameof(request.Email));
            }

            if (string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                throw new ArgumentException("Password hash is required", nameof(request.PasswordHash));
            }

            if (string.IsNullOrWhiteSpace(request.Role))
            {
                throw new ArgumentException("Role is required", nameof(request.Role));
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Check if username already exists
            if (await userRepository.UserExistsAsync(request.Username))
            {
                throw new InvalidOperationException($"Username '{request.Username}' is already taken");
            }

            // Check if email already exists
            if (await userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException($"Email '{request.Email}' is already registered");
            }

            try
            {
                // Create a new user entity
                var user = new User(
                    username: request.Username,
                    email: request.Email,
                    passwordHash: request.PasswordHash,
                    role: request.Role
                );

                // Add to repository
                var userId = await userRepository.AddAsync(user);
                return userId;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating user", ex);
            }
        }
    }
}