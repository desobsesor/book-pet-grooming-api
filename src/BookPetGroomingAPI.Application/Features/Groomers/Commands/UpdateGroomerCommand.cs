using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

public class UpdateGroomerCommand(int groomerId, string name, string specialization) : IRequest<int>
{
    public int GroomerId { get; } = groomerId;
    public string Name { get; } = name;
    public string Specialization { get; } = specialization;
}