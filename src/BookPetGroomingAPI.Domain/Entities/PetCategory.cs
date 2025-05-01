using System;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a category for pets (e.g., Dog, Cat).
    /// </summary>
    public class PetCategory
    {
        public int CategoryId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private PetCategory() { }

        public PetCategory(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The category name cannot be empty", nameof(name));
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateInformation(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The category name cannot be empty", nameof(name));
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}