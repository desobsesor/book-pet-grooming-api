using System;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Domain.Entities
{
    public class Groomer
    {
        public int GroomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Customer>? Customers { get; set; }
    }
}