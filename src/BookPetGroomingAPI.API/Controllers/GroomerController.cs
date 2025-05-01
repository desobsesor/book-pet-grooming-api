using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Groomers.Commands;
using BookPetGroomingAPI.Application.Features.Groomers.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class GroomerController : ApiControllerBase
{
    public GroomerController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves all groomers
    /// </summary>
    /// <returns>List of groomers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GroomerDto>>> GetGroomers()
    {
        var query = new GetGroomersQuery();
        var groomers = await Mediator(query);
        return Ok(groomers);
    }

    /// <summary>
    /// Creates a new groomer
    /// </summary>
    /// <param name="command">Groomer data to create</param>
    /// <returns>Created groomer ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateGroomer([FromBody] CreateGroomerCommand command)
    {
        var groomerId = await Mediator(command);
        return CreatedAtAction(nameof(GetGroomerById), new { id = groomerId }, groomerId);
    }

    /// <summary>
    /// Retrieves a groomer by its ID
    /// </summary>
    /// <param name="id">Groomer ID</param>
    /// <returns>Groomer data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroomerDto>> GetGroomerById(int id)
    {
        var query = new GetGroomerByIdQuery(id);
        var groomer = await Mediator(query);
        return Ok(groomer);
    }


    /// <summary>
    /// Updates an existing groomer
    /// </summary>
    /// <param name="command">Groomer data to update</param>
    /// <returns>No content</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGroomer([FromBody] UpdateGroomerCommand command)
    {
        await Mediator(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a groomer by its ID
    /// </summary>
    /// <param name="id">Groomer ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGroomer(int id)
    {
        var command = new DeleteGroomerCommand(id);
        await Mediator(command);
        return NoContent();
    }
}