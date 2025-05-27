using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler to process the GetAppointmentsByAppointmentDateQuery query
    /// and return the list of appointments by date
    /// </summary>
    public class GetAppointmentsByAppointmentDateQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        : IRequestHandler<GetAppointmentsByAppointmentDateQuery, List<AppointmentDto>>
    {
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByAppointmentDateQuery request, CancellationToken cancellationToken)
        {
            var appointments = await appointmentRepository.GetByAppointmentDateAsync(request.AppointmentDate);
            return mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}