using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Commands
{
    public class CreatePetCommand(string name, decimal weight, string gender, DateTime dateOfBirth, int customerId, int breedId, int categoryId, string allergies, string notes) : IRequest<int>
    {
        public string Name { get; set; } = name;
        public decimal Weight { get; set; } = weight;
        public string Gender { get; set; } = gender;
        public DateTime DateOfBirth { get; set; } = dateOfBirth;
        public int BreedId { get; set; } = breedId;
        public int CategoryId { get; set; } = categoryId;
        public int CustomerId { get; set; } = customerId;
        public string Allergies { get; set; } = allergies;
        public string Notes { get; set; } = notes;
    }
}