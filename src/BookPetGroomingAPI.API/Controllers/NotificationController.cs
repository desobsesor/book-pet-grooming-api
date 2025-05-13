using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Notifications.Commands;
using BookPetGroomingAPI.Application.Features.Notifications.Queries;

namespace BookPetGroomingAPI.API.Controllers;

[Route("api/notifications")]
public class NotificationController : ApiControllerBase
{
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(IMediator mediator, ILogger<NotificationController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all notifications
    /// </summary>
    /// <returns>List of notifications</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        try
        {
            _logger.LogInformation("Getting list of all notifications");

            var query = new GetNotificationsQuery();
            var notifications = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} notifications", notifications.Count);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving notification list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new notification
    /// </summary>
    /// <param name="command">Notification data to create</param>
    /// <returns>Created notification ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        try
        {
            _logger.LogInformation("Starting notification creation: {Message}", command.Message);

            var notificationId = await Mediator(command);

            _logger.LogInformation("Notification successfully created with ID: {NotificationId}", notificationId);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notificationId }, notificationId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating notification. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a notification by its ID
    /// </summary>
    /// <param name="id">Notification ID</param>
    /// <returns>Notification data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<NotificationDto>> GetNotificationById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for notification with ID: {NotificationId}", id);

            var query = new GetNotificationByIdQuery(id);
            var notification = await Mediator(query);

            _logger.LogInformation("Notification with ID: {NotificationId} successfully retrieved", id);
            return Ok(notification);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving notification with ID: {NotificationId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Updates an existing notification
    /// </summary>
    /// <param name="command">Notification data to update</param>
    /// <returns>No content</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationCommand command)
    {
        await Mediator(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a notification by its ID
    /// </summary>
    /// <param name="id">Notification ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var command = new DeleteNotificationCommand(id);
        await Mediator(command);
        return NoContent();
    }
}