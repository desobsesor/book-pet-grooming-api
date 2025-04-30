using System;

namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a notification sent to a customer or groomer.
    /// </summary>
    public class Notification
    {
        public int NotificationId { get; set; }
        public int? CustomerId { get; set; }
        public int? GroomerId { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Navigation properties
        public Customer? Customer { get; set; }
        public Groomer? Groomer { get; set; }
    }
}