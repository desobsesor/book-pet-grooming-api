using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    [Table("customers", Schema = "dbo")]
    public class Customer
    {
        // Properties
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; private set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; private set; }

        [MaxLength(100)]
        [Column("last_name")]
        public string? LastName { get; private set; }

        [MaxLength(150)]
        [Column("email")]
        public string? Email { get; private set; }

        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; private set; }

        [MaxLength(250)]
        [Column("address")]
        public string? Address { get; private set; }

        [Column("preferred_groomer_id")]
        public int? PreferredGroomerId { get; private set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public Groomer? PreferredGroomer { get; private set; }
        public ICollection<Pet>? Pets { get; private set; }

        private Customer() { }

        public Customer(string firstName, string? lastName, string? email, string? phone, string? address, int? preferredGroomerId)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            PreferredGroomerId = preferredGroomerId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactInfo(string? email, string? phone, string? address)
        {
            Email = email;
            Phone = phone;
            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPreferredGroomer(int? groomerId)
        {
            PreferredGroomerId = groomerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string firstName, string? lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            FirstName = firstName;
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}