using BookPetGroomingAPI.Shared.Common;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Query to retrieve a list of appointments belonging to a specific groomer
    /// </summary>
    public class GetAppointmentsByGroomerIdQuery(int groomerId, int pageNumber, int pageSize) : IRequest<PaginatedList<AppointmentDto>>
    {
        /// <summary>
        /// The unique identifier of the groomer whose appointments are being requested 
        /// </summary>
        public int GroomerId { get; } = groomerId;
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }
}