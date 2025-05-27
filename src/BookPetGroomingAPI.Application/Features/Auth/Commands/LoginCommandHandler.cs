using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookPetGroomingAPI.Application.Features.Auth.Commands
{
    /// <summary>
    /// Handler to process the login command and generate a JWT token
    /// </summary>
    /// <remarks>
    /// Constructor of the login handler
    /// </remarks>
    /// <param name="userRepository">User repository</param>
    /// <param name="sessionRepository">Session repository</param>
    /// <param name="configuration">Application configuration</param>
    public class LoginCommandHandler(
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IConfiguration configuration) : IRequestHandler<LoginCommand, LoginResponse?>
    {

        /// <summary>
        /// Handles the login request
        /// </summary>
        /// <param name="request">Command with login credentials</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response with JWT token or null if credentials are invalid</returns>
        public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Get all users and find the one matching the username
            var users = await userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == request.Username);

            // Verify if the user exists and the password is correct
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);
            var expiration = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

            // Create a new session in the database
            var session = new Session(
                userId: user.UserId,
                token: token,
                ipAddress: null, // Could be obtained from HttpContext in the controller
                userAgent: null, // Could be obtained from HttpContext in the controller
                expiresAt: expiration
            //isActive: true
            );

            await sessionRepository.AddAsync(session);

            // Return the response with token and user data
            return new LoginResponse
            {
                Token = token,
                Expiration = expiration,
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role
            };
        }

        /// <summary>
        /// Verifies if the provided password matches the stored hash
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="storedHash">Hash stored in the database</param>
        /// <returns>True if the password is correct, false otherwise</returns>
        private bool VerifyPassword(string password, string storedHash)
        {
            // In a real environment, BCrypt, Argon2 or another secure algorithm would be used here
            // To simplify, we assume the hash is the password itself (DO NOT USE IN PRODUCTION)
            // Could be obtained from HttpContext in the controller
            return password == storedHash;
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user
        /// </summary>
        /// <param name="user">Authenticated user</param>
        /// <returns>Generated JWT token</returns>
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment12345678901234");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}