using AutoMapper;
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
    public class GetAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all appointments
        /// </summary>
        /// <param name="request">The GetAppointmentsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of appointment DTOs</returns>
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await appointmentRepository.GetAllAsync();
            return mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}