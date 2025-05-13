using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsQuery and returning the list of appointments
    /// </summary>
    public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetAppointmentsQueryHandler
        /// </summary>
        /// <param name="appointmentRepository">Repository for appointment operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all appointments
        /// </summary>
        /// <param name="request">The GetAppointmentsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of appointment DTOs</returns>
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}