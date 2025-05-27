using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Commands
{
    /// <summary>
    /// Handler to process the Delete Appointment Command and delete an appointment by ID
    /// </summary>
    public class DeleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        : IRequestHandler<DeleteAppointmentCommand, int>
    {
        public async Task<int> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            await appointmentRepository.DeleteAsync(request.AppointmentId);
            return request.AppointmentId;
        }
    }
}