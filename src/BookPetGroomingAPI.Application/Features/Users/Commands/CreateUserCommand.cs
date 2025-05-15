using MediatR;

namespace BookPetGroomingAPI.Application.Features.Users.Commands
{
    /// <summary>
    /// Command to create a new user in the system
    /// </summary>
    /// <param name="username">Username for login</param>
    /// <param name="email">Email address</param>
    /// <param name="passwordHash">Hashed password</param>
    /// <param name="role">User role (admin, groomer, customer)</param>
    public class CreateUserCommand(string username, string email, string passwordHash, string role) : IRequest<int>
    {
        /// <summary>
        /// Username for login
        /// </summary>
        public string Username { get; set; } = username;

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = email;

        /// <summary>
        /// Hashed password
        /// </summary>
        public string PasswordHash { get; set; } = passwordHash;

        /// <summary>
        /// User role (admin, groomer, customer)
        /// </summary>
        public string Role { get; set; } = role;
    }
}