using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Commands
{
    public class CreateBreedCommand(string name, string breed, int age) : IRequest<int>
    {
        public string Name { get; set; } = name;
        public string Breed { get; set; } = breed;
        public int Age { get; set; } = age;
    }
}