using System;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a pet owned by a customer.
    /// </summary>
    public class Pet
    {
        public int PetId { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public int CustomerId { get; set; }
        public int BreedId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Customer? Owner { get; set; }
        public Breed? Breed { get; set; }
        public PetCategory? Category { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}