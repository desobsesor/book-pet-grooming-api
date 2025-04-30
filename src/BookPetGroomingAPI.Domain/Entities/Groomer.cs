using System;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Domain.Entities
{
    public class Groomer
    {
        public int GroomerId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Specialization { get; private set; }
        public int YearsOfExperience { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
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