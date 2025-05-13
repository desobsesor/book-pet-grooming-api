using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    [Table("groomers", Schema = "dbo")]
    public class Groomer
    {
        [Key]
        [Column("groomer_id")]
        public int GroomerId { get; private set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; private set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string LastName { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Specialization { get; private set; }

        [Column("years_of_experience")]
        public int YearsOfExperience { get; private set; }

        [Column("is_active")]
        public bool IsActive { get; private set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public ICollection<Customer>? Customers { get; private set; }

        private Groomer() { }

        public Groomer(string firstName, string lastName, string? email, string? phone, string? specialization, int yearsOfExperience)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Specialization = specialization;
            YearsOfExperience = yearsOfExperience;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactInfo(string? email, string? phone)
        {
            Email = email;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSpecialization(string? specialization)
        {
            Specialization = specialization;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateExperience(int yearsOfExperience)
        {
            YearsOfExperience = yearsOfExperience;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}