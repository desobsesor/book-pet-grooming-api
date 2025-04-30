using System;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a grooming appointment for a pet.
    /// </summary>
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PetId { get; set; }
        public int GroomerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public required string Status { get; set; }
        public required string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Pet? Pet { get; set; }
        public Groomer? Groomer { get; set; }
    }
}