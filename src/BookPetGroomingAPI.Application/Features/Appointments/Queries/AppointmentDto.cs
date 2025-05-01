namespace BookPetGroomingAPI.Application.Features.Appointments.Queries;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int PetId { get; set; }
    public int GroomerId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public required string Status { get; set; }
    public required string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}