using BookPetGroomingAPI.Shared.Common;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    public class GetAppointmentsQuery : IRequest<PaginatedList<AppointmentDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}