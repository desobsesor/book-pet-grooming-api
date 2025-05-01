namespace BookPetGroomingAPI.Application.Features.Groomers.Queries;

public class GroomerDto
{
    public int GroomerId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}