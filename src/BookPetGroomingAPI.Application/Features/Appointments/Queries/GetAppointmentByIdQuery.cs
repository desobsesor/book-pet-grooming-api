using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    public class GetAppointmentByIdQuery(int appointmentId) : IRequest<AppointmentDto>
    {
        public int AppointmentId { get; } = appointmentId;
    }
}