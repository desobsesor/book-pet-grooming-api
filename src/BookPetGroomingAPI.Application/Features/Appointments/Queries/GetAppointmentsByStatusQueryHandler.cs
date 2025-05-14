using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsByStatusQuery and returning the list of appointments by status
    /// </summary>
    public class GetAppointmentsByStatusQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsByStatusQuery, List<AppointmentDto>>
    {
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByStatusQuery request, CancellationToken cancellationToken)
        {
            var appointments = await appointmentRepository.GetByStatusAsync(request.Status);
            return mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}