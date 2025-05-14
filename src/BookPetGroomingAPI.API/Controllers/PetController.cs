using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Pets.Commands;
using BookPetGroomingAPI.Application.Features.Pets.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[Route("api/pets")]
public class PetController : ApiControllerBase
{
    private readonly ILogger<PetController> _logger;

    public PetController(IMediator mediator, ILogger<PetController> logger) : base(mediator)
    {
        _logger = logger;
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

    /// <summary>
    /// Retrieves all pets for a specific customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>List of pets</returns>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<PetDto>>> GetPetsByCustomerId(int customerId)
    {
        var query = new GetPetsByCustomerIdQuery(customerId);
        var pets = await Mediator(query);
        return Ok(pets);
    }

}