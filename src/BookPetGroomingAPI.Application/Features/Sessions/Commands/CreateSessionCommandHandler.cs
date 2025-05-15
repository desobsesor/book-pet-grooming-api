using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Commands
{
    /// <summary>
    /// Handler for processing the CreateSessionCommand and creating a new session
    /// </summary>
    /// <remarks>
    /// Constructor for CreateSessionCommandHandler
    /// </remarks>
    /// <param name="sessionRepository">Repository for session operations</param>
    /// <param name="userRepository">Repository for user operations</param>
    /// <param name="mapper">Mapping service</param>
    public class CreateSessionCommandHandler(ISessionRepository sessionRepository, IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateSessionCommand, int>
    {

        /// <summary>
        /// Handles the command to create a new session
        /// </summary>
        /// <param name="request">The CreateSessionCommand with session data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created session</returns>
        public async Task<int> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                throw new ArgumentException("Token is required", nameof(request.Token));
            }

            // Verify user exists
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {request.UserId} not found", nameof(request.UserId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new session entity using the domain constructor
                var session = new Session(
                    userId: request.UserId,
                    token: request.Token,
                    expiresAt: request.ExpiresAt,
                    ipAddress: request.IpAddress,
                    userAgent: request.UserAgent);

                // Add the session to the repository
                var sessionId = await sessionRepository.AddAsync(session);

                return sessionId;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating session", ex);
            }
        }
    }
}