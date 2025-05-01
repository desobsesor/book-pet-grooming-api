namespace BookPetGroomingAPI.Application.Features.Pets.Queries;

public class PetDto
{
    public int PetId { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Gender { get; set; }
    public int CustomerId { get; set; }
    public int BreedId { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}