using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands
{
    public class CreateGroomerCommand(string name, string specialization) : IRequest<int>
    {
        public string Name { get; set; } = name;
        public string Specialization { get; set; } = specialization;
    }
}