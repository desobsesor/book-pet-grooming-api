using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    /// <summary>
    /// Handler for processing the GetAppointmentsByPetIdQuery and returning the list of appointments for a pet
    /// </summary>
    public class GetAppointmentsByPetIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<GetAppointmentsByPetIdQuery, List<AppointmentDto>>
    {
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByPetIdQuery request, CancellationToken cancellationToken)
        {
            var appointments = await appointmentRepository.GetByPetIdAsync(request.PetId);
            return mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}