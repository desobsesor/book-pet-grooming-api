using BookPetGroomingAPI.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookPetGroomingAPI.API.Controllers;

/// <summary>
/// Controller to manage user authentication
/// </summary>
[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Authentication controller constructor
    /// </summary>
    /// <param name="mediator">Instance of MediatR for the mediator pattern</param>
    /// <param name="logger">Logging service</param>
    public AuthController(IMediator mediator, ILogger<AuthController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs in a user and generates a JWT token
    /// </summary>
    /// <param name="command">Command with login credentials</param>
    /// <returns>JWT token and session data</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Username}", command.Username);

            var response = await Mediator(command);

            if (response == null)
            {
                _logger.LogWarning("Invalid credentials for user: {Username}", command.Username);
                return Unauthorized(new { error = "Invalid credentials" });
            }

            _logger.LogInformation("Successful login for user: {Username}", command.Username);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error during login. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}