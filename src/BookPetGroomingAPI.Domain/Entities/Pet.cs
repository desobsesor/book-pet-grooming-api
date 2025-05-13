namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a pet owned by a customer.
    /// </summary>
    public class Pet
    {
        public int PetId { get; private set; }
        public string Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public int CustomerId { get; private set; }
        public int BreedId { get; private set; }
        public int CategoryId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public Customer? Owner { get; private set; }
        public Breed? Breed { get; private set; }
        public PetCategory? Category { get; private set; }
        public ICollection<Appointment>? Appointments { get; private set; }

        private Pet() { }

        public Pet(string name, DateTime dateOfBirth, string gender, int customerId, int breedId, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender cannot be empty", nameof(gender));
            Name = name;
            DateOfBirth = dateOfBirth;
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