using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

public class DeleteGroomerCommand(int id) : IRequest<int>
{
    public int Id { get; } = id;
}