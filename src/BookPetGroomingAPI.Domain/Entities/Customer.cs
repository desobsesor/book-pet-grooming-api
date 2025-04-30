using System;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? PreferredGroomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Groomer? PreferredGroomer { get; set; }
        public ICollection<Pet>? Pets { get; set; }
    }
}