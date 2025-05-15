using MediatR;

namespace BookPetGroomingAPI.Application.Features.Auth.Commands
{
    /// <summary>
    /// Command to log in a user and generate a JWT token
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">User password</param>
    public class LoginCommand : IRequest<LoginResponse>
    {
        /// <summary>
        /// Username for login
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Login command response
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Generated JWT token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration date and time
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Authenticated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// User role
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}