using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

/// <summary>
/// Command to create a new groomer in the system
/// </summary>
public record CreateGroomerCommand(string Name, string Specialization) : IRequest<int>
{
    /// <summary>
    /// Gets the name of the groomer
    /// </summary>
    public string Name { get; } = Name;

    /// <summary>
    /// Gets the specialization of the groomer
    /// </summary>
    public string Specialization { get; } = Specialization;
}