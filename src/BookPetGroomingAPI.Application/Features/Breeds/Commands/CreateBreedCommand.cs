using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Commands
{
    public class CreateBreedCommand(string name, string species, string coatType, int groomingDifficulty) : IRequest<int>
    {
        public string Name { get; set; } = name;
        public string Species { get; set; } = species;
        public string CoatType { get; set; } = coatType;
        public int GroomingDifficulty { get; set; } = groomingDifficulty;
    }
}