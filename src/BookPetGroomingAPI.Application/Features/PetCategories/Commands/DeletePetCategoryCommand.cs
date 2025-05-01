using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Commands
{
    public class DeletePetCategoryCommand(int id) : IRequest
    {
        public int Id { get; set; } = id;
    }
}