using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Breeds.Commands;
using BookPetGroomingAPI.Application.Features.Breeds.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class BreedController(IMediator mediator) : ApiControllerBase(mediator)
{

    /// <summary>
    /// Retrieves all breeds
    /// </summary>
    /// <returns>List of breeds</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BreedDto>>> GetBreeds()
    {
        var query = new GetBreedsQuery();
        var breeds = await Mediator(query);
        return Ok(breeds);
    }

    /// <summary>
    /// Creates a new breed
    /// </summary>
    /// <param name="command">Breed data to create</param>
    /// <returns>Created breed ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateBreed([FromBody] CreateBreedCommand command)
    {
        var breedId = await Mediator(command);
        return CreatedAtAction(nameof(GetBreedById), new { id = breedId }, breedId);
    }

    /// <summary>
    /// Retrieves a breed by its ID
    /// </summary>
    /// <param name="id">Breed ID</param>
    /// <returns>Breed data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BreedDto>> GetBreedById(int id)
    {
        var query = new GetBreedByIdQuery(id);
        var breed = await Mediator(query);
        return Ok(breed);
    }
}