using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.PetCategories.Commands;
using BookPetGroomingAPI.Application.Features.PetCategories.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class PetCategoryController : ApiControllerBase
{
    public PetCategoryController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves all pet categories
    /// </summary>
    /// <returns>List of pet categories</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PetCategoryDto>>> GetPetCategories()
    {
        var query = new GetPetCategoriesQuery();
        var petCategories = await Mediator(query);
        return Ok(petCategories);
    }

    /// <summary>
    /// Creates a new pet category
    /// </summary>
    /// <param name="command">Pet category data to create</param>
    /// <returns>Created pet category ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePetCategory([FromBody] CreatePetCategoryCommand command)
    {
        var petCategoryId = await Mediator(command);
        return CreatedAtAction(nameof(GetPetCategoryById), new { id = petCategoryId }, petCategoryId);
    }

    /// <summary>
    /// Retrieves a pet category by its ID
    /// </summary>
    /// <param name="id">Pet category ID</param>
    /// <returns>Pet category data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetCategoryDto>> GetPetCategoryById(int id)
    {
        var query = new GetPetCategoryByIdQuery(id);
        var petCategory = await Mediator(query);
        return Ok(petCategory);
    }
}