using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a notification sent to a customer or groomer.
    /// </summary>
    public class Notification
    {
        [Key]
        [Column("notification_id")]
        public int NotificationId { get; private set; }

        [Column("appointment_id")]
        public int? AppointmentId { get; private set; }

        [Column("recipient_type")]
        public string RecipientType { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;

        [Column("is_read")]
        public bool IsRead { get; private set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        // Navigation properties
        public Appointment? Appointment { get; private set; }

        private Notification() { }

        public Notification(int? appointmentId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The notification message cannot be empty", nameof(message));
            AppointmentId = appointmentId;
            Message = message;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void UpdateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The notification message cannot be empty", nameof(message));
            Message = message;
        }
    }
}