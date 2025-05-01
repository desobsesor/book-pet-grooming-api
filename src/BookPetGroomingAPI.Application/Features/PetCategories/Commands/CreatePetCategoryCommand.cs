using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Commands
{
    public record CreatePetCategoryCommand(string Name, string Description) : IRequest<int>
    {
        public string Name { get; set; } = Name;
        public string Description { get; set; } = Description;
    }
}