using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a user session in the system
    /// </summary>
    [Table("sessions", Schema = "dbo")]
    public class Session
    {
        /// <summary>
        /// The unique identifier for the session
        /// </summary>
        [Key]
        [Column("session_id")]
        public int SessionId { get; private set; }

        /// <summary>
        /// The user ID associated with this session
        /// </summary>
        [Required]
        [Column("user_id")]
        public int UserId { get; private set; }

        /// <summary>
        /// The authentication token for the session
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("token")]
        public string Token { get; private set; }

        /// <summary>
        /// The IP address from which the session was created
        /// </summary>
        [MaxLength(50)]
        [Column("ip_address")]
        public string? IpAddress { get; private set; }

        /// <summary>
        /// The user agent (browser/device) from which the session was created
        /// </summary>
        [MaxLength(255)]
        [Column("user_agent")]
        public string? UserAgent { get; private set; }

        /// <summary>
        /// The date and time when the session expires
        /// </summary>
        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; private set; }

        /// <summary>
        /// The date and time when the session was created
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// The date and time when the session was last updated
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        /// <summary>
        /// Navigation property for the associated user
        /// </summary>
        [ForeignKey("UserId")]
        public User? User { get; private set; }

        // Required by EF Core
        private Session() { }

        /// <summary>
        /// Creates a new session
        /// </summary>
        /// <param name="userId">The ID of the user associated with this session</param>
        /// <param name="token">The authentication token</param>
        /// <param name="expiresAt">The expiration date and time</param>
        /// <param name="ipAddress">The IP address (optional)</param>
        /// <param name="userAgent">The user agent (optional)</param>
        public Session(int userId, string token, DateTime expiresAt, string? ipAddress = null, string? userAgent = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be empty", nameof(token));
            }

            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the session expiration time
        /// </summary>
        /// <param name="newExpiresAt">The new expiration date and time</param>
        public void UpdateExpiration(DateTime newExpiresAt)
        {
            ExpiresAt = newExpiresAt;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if the session is expired
        /// </summary>
        /// <returns>True if the session is expired, false otherwise</returns>
        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }
    }
}