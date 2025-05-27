using AutoMapper;
using BookPetGroomingAPI.Shared.Common;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsQuery and returning the list of appointments
    /// </summary>
    /// <remarks>
    /// Constructor for GetAppointmentsQueryHandler
    /// </remarks>
    /// <param name="appointmentRepository">Repository for appointment operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsQuery, PaginatedList<AppointmentDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all appointments
        /// </summary>
        /// <param name="request">The GetAppointmentsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of appointment DTOs</returns>
        public async Task<PaginatedList<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = appointmentRepository.GetAllQueryable();
            var paginatedAppointments = await PaginatedList<AppointmentDto>.CreateAsync(
                query.Select(a => mapper.Map<AppointmentDto>(a)),
                request.PageNumber,
                request.PageSize
            );

            return paginatedAppointments;
        }
    }
}