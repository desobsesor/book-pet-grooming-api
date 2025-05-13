using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

public class DeleteGroomerCommand(int groomerId) : IRequest<int>
{
    public int GroomerId { get; } = groomerId;
}