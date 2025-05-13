using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands;

/// <summary>
/// Command to create a new groomer in the system
/// </summary>
public record CreateGroomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Specialization,
    int YearsOfExperience,
    bool IsActive = true) : IRequest<int>
{
    /// <summary>
    /// Gets the first name of the groomer
    /// </summary>
    public string FirstName { get; } = FirstName;

    /// <summary>
    /// Gets the last name of the groomer
    /// </summary>
    public string LastName { get; } = LastName;

    /// <summary>
    /// Gets the email of the groomer
    /// </summary>
    public string Email { get; } = Email;

    /// <summary>
    /// Gets the phone number of the groomer
    /// </summary>
    public string Phone { get; } = Phone;

    /// <summary>
    /// Gets the specialization of the groomer
    /// </summary>
    public string Specialization { get; } = Specialization;

    /// <summary>
    /// Gets the years of experience of the groomer
    /// </summary>
    public int YearsOfExperience { get; } = YearsOfExperience;

    /// <summary>
    /// Gets whether the groomer is active
    /// </summary>
    public bool IsActive { get; } = IsActive;
}