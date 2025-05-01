using System;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; private set; }
        public string FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public int? PreferredGroomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
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