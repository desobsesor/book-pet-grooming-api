using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a pet owned by a customer.
    /// </summary>
    public class Pet
    {
        [Key]
        [Column("pet_id")]
        public int PetId { get; private set; }
        public string Name { get; private set; }
        public decimal Weight { get; private set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }

        [Column("customer_id")]
        public int CustomerId { get; private set; }

        [Column("breed_id")]
        public int BreedId { get; private set; }

        [Column("category_id")]
        public int CategoryId { get; private set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public Customer? Owner { get; private set; }
        public Breed? Breed { get; private set; }
        public PetCategory? Category { get; private set; }
        public ICollection<Appointment>? Appointments { get; private set; }

        private Pet() { }

        public Pet(string name, decimal weight, DateTime dateOfBirth, string gender, int customerId, int breedId, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender cannot be empty", nameof(gender));
            Name = name;
            DateOfBirth = dateOfBirth;
            Weight = weight;
            Gender = gender;
            CustomerId = customerId;
            BreedId = breedId;
            CategoryId = categoryId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender cannot be empty", nameof(gender));
            Gender = gender;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBreed(int breedId)
        {
            BreedId = breedId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCategory(int categoryId)
        {
            CategoryId = categoryId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}