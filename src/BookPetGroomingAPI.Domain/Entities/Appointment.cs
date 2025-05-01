using System;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a grooming appointment for a pet.
    /// </summary>
    public class Appointment
    {
        public int AppointmentId { get; private set; }
        public int PetId { get; private set; }
        public int GroomerId { get; private set; }
        public DateTime AppointmentDate { get; private set; }
        public string Status { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public Pet? Pet { get; private set; }
        public Groomer? Groomer { get; private set; }

        private Appointment() { }

        public Appointment(int petId, int groomerId, DateTime appointmentDate, string status, string notes)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty", nameof(status));
            if (string.IsNullOrWhiteSpace(notes))
                throw new ArgumentException("Notes cannot be empty", nameof(notes));
            PetId = petId;
            GroomerId = groomerId;
            AppointmentDate = appointmentDate;
            Status = status;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty", nameof(status));
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                throw new ArgumentException("Notes cannot be empty", nameof(notes));
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newDate)
        {
            AppointmentDate = newDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}