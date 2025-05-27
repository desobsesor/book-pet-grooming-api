using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsByGroomerIdQuery and returning the list of appointments for a groomer
    /// </summary>
    public class GetAppointmentsByGroomerIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsByGroomerIdQuery, List<AppointmentDto>>
    {
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByGroomerIdQuery request, CancellationToken cancellationToken)
        {
            var pets = await appointmentRepository.GetByGroomerIdAsync(request.GroomerId);
            return mapper.Map<List<AppointmentDto>>(pets);
        }
    }
}