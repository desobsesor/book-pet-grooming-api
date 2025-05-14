using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Query to retrieve a list of appointments belonging to a specific pet
    /// </summary>
    public class GetAppointmentsByPetIdQuery(int petId) : IRequest<List<AppointmentDto>>
    {
        /// <summary>
        /// The unique identifier of the pet whose appointments are being requested 
        /// </summary>
        public int PetId { get; } = petId;
    }
}