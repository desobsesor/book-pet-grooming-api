using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsByGroomerIdQuery and returning the list of appointments for a groomer
    /// </summary>
    public class GetAppointmentsByGroomerIdQueryHandler : IRequestHandler<GetAppointmentsByGroomerIdQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public GetAppointmentsByGroomerIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByGroomerIdQuery request, CancellationToken cancellationToken)
        {
            var pets = await _appointmentRepository.GetByGroomerIdAsync(request.GroomerId);
            return _mapper.Map<List<AppointmentDto>>(pets);
        }
    }
}