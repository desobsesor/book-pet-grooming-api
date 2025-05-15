using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Queries
{
    /// <summary>
    /// Query to retrieve all users from the system
    /// </summary>
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }
}