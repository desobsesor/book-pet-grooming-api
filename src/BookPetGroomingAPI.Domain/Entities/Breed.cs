namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a breed for pets (e.g., Labrador, Persian).
    /// </summary>
    public class Breed
    {
        public int BreedId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Species { get; private set; } = string.Empty;
        public string CoatType { get; private set; } = string.Empty;
        public int GroomingDifficulty { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Breed() { }

        public Breed(string name, string species, string coatType, int groomingDifficulty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The breed name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(species))
                throw new ArgumentException("The species cannot be empty", nameof(species));
            if (groomingDifficulty < 0)
                throw new ArgumentException("Grooming difficulty cannot be negative", nameof(groomingDifficulty));
            Name = name;
            Species = species;
            CoatType = coatType;
            GroomingDifficulty = groomingDifficulty;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateInformation(string name, string species, string coatType, int groomingDifficulty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The breed name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(species))
                throw new ArgumentException("The species cannot be empty", nameof(species));
            if (groomingDifficulty < 0)
                throw new ArgumentException("Grooming difficulty cannot be negative", nameof(groomingDifficulty));
            Name = name;
            Species = species;
            CoatType = coatType;
            GroomingDifficulty = groomingDifficulty;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}