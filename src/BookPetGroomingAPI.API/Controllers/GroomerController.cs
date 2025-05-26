using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Groomers.Commands;
using BookPetGroomingAPI.Application.Features.Groomers.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/groomers")]
public class GroomerController : ApiControllerBase
{
    private readonly ILogger<GroomerController> _logger;

    public GroomerController(IMediator mediator, ILogger<GroomerController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all groomers
    /// </summary>
    /// <returns>List of groomers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GroomerDto>>> GetGroomers()
    {
        try
        {
            _logger.LogInformation("Getting list of all groomers");

            var query = new GetGroomersQuery();
            var groomers = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} groomers", groomers.Count);
            return Ok(groomers);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving groomer list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new groomer
    /// </summary>
    /// <param name="command">Groomer data to create</param>
    /// <returns>Created groomer ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateGroomer([FromBody] CreateGroomerCommand command)
    {
        try
        {
            _logger.LogInformation("Starting groomer creation: {FirstName}", command.FirstName);

            var groomerId = await Mediator(command);

            _logger.LogInformation("Groomer successfully created with ID: {GroomerId}", groomerId);
            return CreatedAtAction(nameof(GetGroomerById), new { id = groomerId }, groomerId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating groomer {FirstName}. RequestId: {RequestId}. Details: {Message}",
                command.FirstName, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a groomer by its ID
    /// </summary>
    /// <param name="id">Groomer ID</param>
    /// <returns>Groomer data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GroomerDto>> GetGroomerById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for groomer with ID: {GroomerId}", id);

            var query = new GetGroomerByIdQuery(id);
            var groomer = await Mediator(query);

            _logger.LogInformation("Groomer with ID: {GroomerId} successfully retrieved", id);
            return Ok(groomer);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving groomer with ID: {GroomerId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }


    /// <summary>
    /// Updates an existing groomer
    /// </summary>
    /// <param name="command">Groomer data to update</param>
    /// <returns>No content</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateGroomer([FromBody] UpdateGroomerCommand command)
    {
        try
        {
            _logger.LogInformation("Updating groomer with ID: {GroomerId}", command.GroomerId);

            await Mediator(command);

            _logger.LogInformation("Groomer with ID: {GroomerId} successfully updated", command.GroomerId);
            return NoContent();
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error updating groomer with ID: {GroomerId}. RequestId: {RequestId}. Details: {Message}",
                command.GroomerId, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Deletes a groomer by its ID
    /// </summary>
    /// <param name="id">Groomer ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteGroomer(int id)
    {
        try
        {
            _logger.LogInformation("Deleting groomer with ID: {GroomerId}", id);

            var command = new DeleteGroomerCommand(id);
            await Mediator(command);

            _logger.LogInformation("Groomer with ID: {GroomerId} successfully deleted", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error deleting groomer with ID: {GroomerId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}