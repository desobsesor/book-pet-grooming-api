using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Commands
{
    /// <summary>
    /// Command to create a new session in the system
    /// </summary>
    /// <param name="userId">User ID associated with the session</param>
    /// <param name="token">Authentication token</param>
    /// <param name="expiresAt">Expiration date and time</param>
    /// <param name="ipAddress">IP address from which the session was created</param>
    /// <param name="userAgent">User agent (browser/device) from which the session was created</param>
    public class CreateSessionCommand(int userId, string token, DateTime expiresAt, string? ipAddress = null, string? userAgent = null) : IRequest<int>
    {
        /// <summary>
        /// User ID associated with the session
        /// </summary>
        public int UserId { get; set; } = userId;

        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; } = token;

        /// <summary>
        /// Expiration date and time
        /// </summary>
        public DateTime ExpiresAt { get; set; } = expiresAt;

        /// <summary>
        /// IP address from which the session was created
        /// </summary>
        public string? IpAddress { get; set; } = ipAddress;

        /// <summary>
        /// User agent (browser/device) from which the session was created
        /// </summary>
        public string? UserAgent { get; set; } = userAgent;
    }
}