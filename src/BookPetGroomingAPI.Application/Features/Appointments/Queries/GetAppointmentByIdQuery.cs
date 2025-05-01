using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    public class GetAppointmentByIdQuery(int id) : IRequest<AppointmentDto>
    {
        public int Id { get; } = id;
    }
}