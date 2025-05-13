using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a grooming appointment for a pet.
    /// </summary>
    [Table("appointments", Schema = "dbo")]
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public int AppointmentId { get; private set; }

        [Column("pet_id")]
        public int PetId { get; private set; }

        [Column("groomer_id")]
        public int GroomerId { get; private set; }

        [Column("appointment_date")]
        public DateTime AppointmentDate { get; private set; }

        [Column("start_time", TypeName = "time")]
        public TimeOnly StartTime { get; private set; }

        [Column("estimated_duration")]
        public int EstimatedDuration { get; private set; }
        public string Status { get; private set; }
        public decimal Price { get; private set; }
        public string Notes { get; private set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        // Navigation properties
        public Pet? Pet { get; private set; }
        public Groomer? Groomer { get; private set; }

        private Appointment() { }

        public Appointment(int petId, int groomerId, DateTime appointmentDate, TimeOnly startTime, string status, string notes, decimal price)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty", nameof(status));
            if (string.IsNullOrWhiteSpace(notes))
                throw new ArgumentException("Notes cannot be empty", nameof(notes));
            PetId = petId;
            GroomerId = groomerId;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            Status = status;
            Price = price;
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