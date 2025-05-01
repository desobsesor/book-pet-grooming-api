using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Commands
{
    public class UpdatePetCategoryCommand(int petCategoryId, string name, string description) : IRequest
    {
        public int PetCategoryId { get; set; } = petCategoryId;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
    }
}