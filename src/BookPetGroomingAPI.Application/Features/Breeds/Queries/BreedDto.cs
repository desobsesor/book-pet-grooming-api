namespace BookPetGroomingAPI.Application.Features.Breeds.Queries;

public class BreedDto
{
    public int BreedId { get; set; }
    public required string Name { get; set; }
    public required string Species { get; set; }
    public required string CoatType { get; set; }
    public int GroomingDifficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}