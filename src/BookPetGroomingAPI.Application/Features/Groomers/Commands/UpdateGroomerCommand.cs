using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

public class UpdateGroomerCommand(int groomerId, string firstName, string lastName, string specialization) : IRequest<int>
{
    public int GroomerId { get; } = groomerId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string Specialization { get; } = specialization;
}