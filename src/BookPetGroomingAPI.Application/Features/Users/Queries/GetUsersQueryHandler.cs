using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Queries
{
    /// <summary>
    /// Handler for processing the GetUsersQuery and returning the list of users
    /// </summary>
    /// <remarks>
    /// Constructor for GetUsersQueryHandler
    /// </remarks>
    /// <param name="userRepository">Repository for user operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersQuery, List<UserDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all users
        /// </summary>
        /// <param name="request">The GetUsersQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of user DTOs</returns>
        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<List<UserDto>>(users);
        }
    }
}