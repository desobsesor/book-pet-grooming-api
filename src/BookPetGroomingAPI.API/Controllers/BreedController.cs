using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Breeds.Commands;
using BookPetGroomingAPI.Application.Features.Breeds.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/breeds")]
public class BreedController : ApiControllerBase
{
    private readonly ILogger<BreedController> _logger;

    public BreedController(IMediator mediator, ILogger<BreedController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all breeds
    /// </summary>
    /// <returns>List of breeds</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<BreedDto>>> GetBreeds()
    {
        try
        {
            _logger.LogInformation("Getting list of all breeds");

            var query = new GetBreedsQuery();
            var breeds = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} breeds", breeds.Count);
            return Ok(breeds);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving breed list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new breed
    /// </summary>
    /// <param name="command">Breed data to create</param>
    /// <returns>Created breed ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateBreed([FromBody] CreateBreedCommand command)
    {
        try
        {
            _logger.LogInformation("Starting breed creation: {Name}", command.Name);

            var breedId = await Mediator(command);

            _logger.LogInformation("Breed successfully created with ID: {BreedId}", breedId);
            return CreatedAtAction(nameof(GetBreedById), new { id = breedId }, breedId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating breed {Name}. RequestId: {RequestId}. Details: {Message}",
                command.Name, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a breed by its ID
    /// </summary>
    /// <param name="id">Breed ID</param>
    /// <returns>Breed data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BreedDto>> GetBreedById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for breed with ID: {BreedId}", id);

            var query = new GetBreedByIdQuery(id);
            var breed = await Mediator(query);

            _logger.LogInformation("Breed with ID: {BreedId} successfully retrieved", id);
            return Ok(breed);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving breed with ID: {BreedId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}