using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    [Table("users", Schema = "dbo")]
    public class User
    {
        /// <summary>
        /// The unique identifier for the user
        /// </summary>
        [Key]
        [Column("user_id")]
        public int UserId { get; private set; }

        /// <summary>
        /// The username for login
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("username")]
        public string Username { get; private set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; private set; }

        /// <summary>
        /// The hashed password
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; private set; }

        /// <summary>
        /// The user's role in the system
        /// </summary>
        [Required]
        [MaxLength(20)]
        [Column("role")]
        public string Role { get; private set; }

        /// <summary>
        /// The date and time of the user's last login
        /// </summary>
        [Column("last_login")]
        public DateTime? LastLogin { get; private set; }

        /// <summary>
        /// Indicates if the user account is active
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// The date and time when the user was created
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// The date and time when the user was last updated
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        // Private constructor for EF Core
        private User() { }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="email">The email address</param>
        /// <param name="passwordHash">The hashed password</param>
        /// <param name="role">The user role</param>
        public User(string username, string email, string passwordHash, string role)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the user's information
        /// </summary>
        /// <param name="username">The new username</param>
        /// <param name="email">The new email</param>
        /// <param name="role">The new role</param>
        public void Update(string username, string email, string role)
        {
            Username = username;
            Email = email;
            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the user's password
        /// </summary>
        /// <param name="passwordHash">The new password hash</param>
        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Records a login event
        /// </summary>
        public void RecordLogin()
        {
            LastLogin = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Activates the user account
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates the user account
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}