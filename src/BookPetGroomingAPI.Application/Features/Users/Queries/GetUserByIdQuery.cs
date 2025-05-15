using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Queries
{
    /// <summary>
    /// Query to retrieve a user by ID
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve</param>
    public class GetUserByIdQuery(int userId) : IRequest<UserDto>
    {
        /// <summary>
        /// The ID of the user to retrieve
        /// </summary>
        public int UserId { get; } = userId;
    }
}