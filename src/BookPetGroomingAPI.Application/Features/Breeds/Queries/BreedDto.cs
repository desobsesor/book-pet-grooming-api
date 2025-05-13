using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries;

public class BreedDto : IMapFrom<Breed>
{
    public int BreedId { get; set; }
    public required string Name { get; set; }
    public required string Species { get; set; }
    public required string CoatType { get; set; }
    public int GroomingDifficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Breed, BreedDto>();
    }
}