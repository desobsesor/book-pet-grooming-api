using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.PetCategories.Commands;
using BookPetGroomingAPI.Application.Features.PetCategories.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pet-categories")]
public class PetCategoryController : ApiControllerBase
{
    private readonly ILogger<PetCategoryController> _logger;

    public PetCategoryController(IMediator mediator, ILogger<PetCategoryController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all pet categories
    /// </summary>
    /// <returns>List of pet categories</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PetCategoryDto>>> GetPetCategories()
    {
        try
        {
            _logger.LogInformation("Getting list of all pet categories");

            var query = new GetPetCategoriesQuery();
            var petCategories = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} pet categories", petCategories.Count);
            return Ok(petCategories);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving pet category list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new pet category
    /// </summary>
    /// <param name="command">Pet category data to create</param>
    /// <returns>Created pet category ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreatePetCategory([FromBody] CreatePetCategoryCommand command)
    {
        try
        {
            _logger.LogInformation("Starting pet category creation: {Name}", command.Name);

            var petCategoryId = await Mediator(command);

            _logger.LogInformation("Pet category successfully created with ID: {PetCategoryId}", petCategoryId);
            return CreatedAtAction(nameof(GetPetCategoryById), new { id = petCategoryId }, petCategoryId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating pet category {Name}. RequestId: {RequestId}. Details: {Message}",
                command.Name, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a pet category by its ID
    /// </summary>
    /// <param name="id">Pet category ID</param>
    /// <returns>Pet category data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PetCategoryDto>> GetPetCategoryById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for pet category with ID: {PetCategoryId}", id);

            var query = new GetPetCategoryByIdQuery(id);
            var petCategory = await Mediator(query);

            _logger.LogInformation("Pet category with ID: {PetCategoryId} successfully retrieved", id);
            return Ok(petCategory);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving pet category with ID: {PetCategoryId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}