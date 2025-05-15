using BookPetGroomingAPI.Application.Features.Users.Commands;
using BookPetGroomingAPI.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookPetGroomingAPI.API.Controllers;

[Route("api/users")]
public class UserController : ApiControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all users
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        try
        {
            _logger.LogInformation("Getting list of all users");

            var query = new GetUsersQuery();
            var users = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} users", users.Count);
            return Ok(users);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving user list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="command">User data to create</param>
    /// <returns>Created user ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            _logger.LogInformation("Starting user creation: {Username}", command.Username);

            var userId = await Mediator(command);

            _logger.LogInformation("User successfully created with ID: {UserId}", userId);
            return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating user {Username}. RequestId: {RequestId}. Details: {Message}",
                command.Username, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a user by its ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for user with ID: {UserId}", id);

            var query = new GetUserByIdQuery(id);
            var user = await Mediator(query);

            _logger.LogInformation("User with ID: {UserId} successfully retrieved", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving user with ID: {UserId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}