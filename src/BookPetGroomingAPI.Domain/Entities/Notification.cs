namespace BookPetGroomingAPI.Domain.Entities
{
    /// <summary>
    /// Represents a notification sent to a customer or groomer.
    /// </summary>
    public class Notification
    {
        public int NotificationId { get; private set; }
        public int? CustomerId { get; private set; }
        public int? GroomerId { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ReadAt { get; private set; }

        // Navigation properties
        public Customer? Customer { get; private set; }
        public Groomer? Groomer { get; private set; }

        private Notification() { }

        public Notification(int? customerId, int? groomerId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The notification message cannot be empty", nameof(message));
            CustomerId = customerId;
            GroomerId = groomerId;
            Message = message;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTime.UtcNow;
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