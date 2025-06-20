using BookPetGroomingAPI.Application.Features.Appointments.Commands;
using BookPetGroomingAPI.Application.Features.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookPetGroomingAPI.Shared.Common;
namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/appointments")]
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
    public async Task<ActionResult<PaginatedList<AppointmentDto>>> GetAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting list of all appointments");

            var query = new GetAppointmentsQuery { PageNumber = pageNumber, PageSize = pageSize };
            var appointments = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} appointments", appointments.Items.Count);
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

    /// <summary>
    /// Retrieves all appointments for a specific groomer
    /// </summary>
    /// <param name="groomerId">Groomer ID</param>
    /// <returns>List of appointments</returns>
    [HttpGet("groomer/{groomerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedList<AppointmentDto>>> GetAppointmentsByGroomerId(int groomerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAppointmentsByGroomerIdQuery(groomerId, pageNumber, pageSize);
        var appointments = await Mediator(query);
        return Ok(appointments);
    }

    /// <summary>
    /// Retrieves all appointments for a specific groomer
    /// </summary>
    /// <param name="petId">Groomer ID</param>
    /// <returns>List of appointments</returns>
    [HttpGet("pet/{petId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByPetId(int petId)
    {
        var query = new GetAppointmentsByPetIdQuery(petId);
        var appointments = await Mediator(query);
        return Ok(appointments);
    }


    /// <summary>
    /// Retrieves all appointment for a specific appointment date
    /// </summary>
    /// <param name="date">Appointment Date</param>
    /// <returns>List of appointments</returns>
    [HttpGet("appointment-date/{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByAppointmentDate(DateTime date)
    {
        var query = new GetAppointmentsByAppointmentDateQuery(date);
        var appointments = await Mediator(query);
        return Ok(appointments);
    }

    /// <summary>
    /// Retrieves all appointments for a specific status
    /// </summary>
    /// <param name="status">Appointment status to filter (completed, approved, cancelled, pending)</param>
    /// <returns>List of filtered appointments</returns>
    /// <response code="200">Returns the requested appointments</response>
    /// <response code="404">No appointments found with specified status</response>
    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByStatus(string status)
    {
        var query = new GetAppointmentsByStatusQuery(status);
        var appointments = await Mediator(query);
        return Ok(appointments);
    }

    /// <summary>
    /// Deletes a notification by its ID
    /// </summary>
    /// <param name="id">Notification ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var command = new DeleteAppointmentCommand(id);
        await Mediator(command);
        return NoContent();
    }
}