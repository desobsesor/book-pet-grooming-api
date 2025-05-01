using MediatR;
using System.Collections.Generic;

namespace BookPetGroomingAPI.Application.Features.Appointments.Queries
{
    public class GetAppointmentsQuery : IRequest<List<AppointmentDto>>
    {
    }
}