using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Appointments.Commands;
using BookPetGroomingAPI.Application.Features.Appointments.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[Route("api/appointments")]
public class AppointmentController : ApiControllerBase
{
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IMediator mediator, ILogger<AppointmentController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all appointments
    /// </summary>
    /// <returns>List of appointments</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointments()
    {
        try
        {
            _logger.LogInformation("Getting list of all appointments");

            var query = new GetAppointmentsQuery();
            var appointments = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} appointments", appointments.Count);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving appointment list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new appointment
    /// </summary>
    /// <param name="command">Appointment data to create</param>
    /// <returns>Created appointment ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        try
        {
            _logger.LogInformation("Starting appointment creation for pet ID: {PetId} with groomer ID: {GroomerId}", 
                command.PetId, command.GroomerId);

            var appointmentId = await Mediator(command);

            _logger.LogInformation("Appointment successfully created with ID: {AppointmentId}", appointmentId);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, appointmentId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating appointment for pet ID: {PetId} with groomer ID: {GroomerId}. RequestId: {RequestId}. Details: {Message}",
                command.PetId, command.GroomerId, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves an appointment by its ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <returns>Appointment data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for appointment with ID: {AppointmentId}", id);

            var query = new GetAppointmentByIdQuery(id);
            var appointment = await Mediator(query);

            _logger.LogInformation("Appointment with ID: {AppointmentId} successfully retrieved", id);
            return Ok(appointment);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}