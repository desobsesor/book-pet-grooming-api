using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Commands
{
    public class DeleteAppointmentCommand(int appointmentId) : IRequest<int>
    {
        public int AppointmentId { get; } = appointmentId;
    }
}