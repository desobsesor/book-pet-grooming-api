using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentByIdQuery and returning the appointment details
    /// </summary>
    public class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await appointmentRepository.GetByIdAsync(request.AppointmentId);
            return mapper.Map<AppointmentDto>(appointment);
        }
    }
}