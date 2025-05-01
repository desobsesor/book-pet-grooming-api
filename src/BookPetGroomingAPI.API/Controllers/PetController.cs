using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Pets.Commands;
using BookPetGroomingAPI.Application.Features.Pets.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class PetController : ApiControllerBase
{
    public PetController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves all pets
    /// </summary>
    /// <returns>List of pets</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PetDto>>> GetPets()
    {
        var query = new GetPetsQuery();
        var pets = await Mediator(query);
        return Ok(pets);
    }

    /// <summary>
    /// Creates a new pet
    /// </summary>
    /// <param name="command">Pet data to create</param>
    /// <returns>Created pet ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePet([FromBody] CreatePetCommand command)
    {
        var petId = await Mediator(command);
        return CreatedAtAction(nameof(GetPetById), new { id = petId }, petId);
    }

    /// <summary>
    /// Retrieves a pet by its ID
    /// </summary>
    /// <param name="id">Pet ID</param>
    /// <returns>Pet data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetDto>> GetPetById(int id)
    {
        var query = new GetPetByIdQuery(id);
        var pet = await Mediator(query);
        return Ok(pet);
    }
}