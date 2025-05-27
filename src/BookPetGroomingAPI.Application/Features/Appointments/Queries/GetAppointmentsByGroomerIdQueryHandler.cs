using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using BookPetGroomingAPI.Shared.Common;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsByGroomerIdQuery and returning the list of appointments for a groomer
    /// </summary>
    public class GetAppointmentsByGroomerIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsByGroomerIdQuery, PaginatedList<AppointmentDto>>
    {
        public async Task<PaginatedList<AppointmentDto>> Handle(GetAppointmentsByGroomerIdQuery request, CancellationToken cancellationToken)
        {
            var query = appointmentRepository.GetAllQueryable().Where(a => a.GroomerId == request.GroomerId);

            var paginatedAppointments = await PaginatedList<AppointmentDto>.CreateAsync(
                query.Select(a => mapper.Map<AppointmentDto>(a)),
                request.PageNumber, // Default to page 1 if not provided
                request.PageSize  // Default to page size 10 if not provided
            );

            return paginatedAppointments;
        }
    }
}