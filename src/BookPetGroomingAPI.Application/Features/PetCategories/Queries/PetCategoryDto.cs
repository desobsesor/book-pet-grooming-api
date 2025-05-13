using AutoMapper;
using BookPetGroomingAPI.Application.Common.Mappings;
using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Queries;

public class PetCategoryDto : IMapFrom<PetCategory>
{
    public int PetCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<PetCategory, PetCategoryDto>();
    }
}