namespace BookPetGroomingAPI.Application.Features.Notifications.Queries;

public class NotificationDto
{
    public int NotificationId { get; set; }
    public int? CustomerId { get; set; }
    public int? GroomerId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}