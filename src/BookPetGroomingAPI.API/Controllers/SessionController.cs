using BookPetGroomingAPI.Application.Features.Sessions.Commands;
using BookPetGroomingAPI.Application.Features.Sessions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/sessions")]
public class SessionController : ApiControllerBase
{
    private readonly ILogger<SessionController> _logger;

    public SessionController(IMediator mediator, ILogger<SessionController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all sessions
    /// </summary>
    /// <returns>List of sessions</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<SessionDto>>> GetSessions()
    {
        try
        {
            _logger.LogInformation("Getting list of all sessions");

            var query = new GetSessionsQuery();
            var sessions = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} sessions", sessions.Count);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving session list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new session
    /// </summary>
    /// <param name="command">Session data to create</param>
    /// <returns>Created session ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateSession([FromBody] CreateSessionCommand command)
    {
        try
        {
            _logger.LogInformation("Starting session creation for user ID: {UserId}", command.UserId);

            var sessionId = await Mediator(command);

            _logger.LogInformation("Session successfully created with ID: {SessionId}", sessionId);
            return CreatedAtAction(nameof(GetSessionById), new { id = sessionId }, sessionId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating session for user ID: {UserId}. RequestId: {RequestId}. Details: {Message}",
                command.UserId, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a session by its ID
    /// </summary>
    /// <param name="id">Session ID</param>
    /// <returns>Session data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SessionDto>> GetSessionById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for session with ID: {SessionId}", id);

            var query = new GetSessionByIdQuery(id);
            var session = await Mediator(query);

            _logger.LogInformation("Session with ID: {SessionId} successfully retrieved", id);
            return Ok(session);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving session with ID: {SessionId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}