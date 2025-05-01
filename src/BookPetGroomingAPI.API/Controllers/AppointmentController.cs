using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Appointments.Commands;
using BookPetGroomingAPI.Application.Features.Appointments.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class AppointmentController : ApiControllerBase
{
    public AppointmentController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves all appointments
    /// </summary>
    /// <returns>List of appointments</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointments()
    {
        var query = new GetAppointmentsQuery();
        var appointments = await Mediator(query);
        return Ok(appointments);
    }

    /// <summary>
    /// Creates a new appointment
    /// </summary>
    /// <param name="command">Appointment data to create</param>
    /// <returns>Created appointment ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var appointmentId = await Mediator(command);
        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, appointmentId);
    }

    /// <summary>
    /// Retrieves an appointment by its ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <returns>Appointment data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
    {
        var query = new GetAppointmentByIdQuery(id);
        var appointment = await Mediator(query);
        return Ok(appointment);
    }
}