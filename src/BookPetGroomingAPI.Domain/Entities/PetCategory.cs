using System;

namespace BookPetGroomingAPI.Domain.Entities
{
    public class PetCategory
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}