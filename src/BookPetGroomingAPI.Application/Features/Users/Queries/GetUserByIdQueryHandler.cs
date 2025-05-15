using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Queries
{
    /// <summary>
    /// Handler for processing the GetUserByIdQuery and retrieving a user by ID
    /// </summary>
    /// <remarks>
    /// Constructor for GetUserByIdQueryHandler
    /// </remarks>
    /// <param name="userRepository">Repository for user operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDto>
    {

        /// <summary>
        /// Handles the query to retrieve a user by ID
        /// </summary>
        /// <param name="request">The GetUserByIdQuery with user ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>User DTO if found</returns>
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId) ?? throw new KeyNotFoundException($"User with ID {request.UserId} not found");
            return mapper.Map<UserDto>(user);
        }
    }
}