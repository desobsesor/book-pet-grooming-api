using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Notifications.Commands;
using BookPetGroomingAPI.Application.Features.Notifications.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class NotificationController(IMediator mediator) : ApiControllerBase(mediator)
{

    /// <summary>
    /// Retrieves all notifications
    /// </summary>
    /// <returns>List of notifications</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        var query = new GetNotificationsQuery();
        var notifications = await Mediator(query);
        return Ok(notifications);
    }

    /// <summary>
    /// Creates a new notification
    /// </summary>
    /// <param name="command">Notification data to create</param>
    /// <returns>Created notification ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        var notificationId = await Mediator(command);
        return CreatedAtAction(nameof(GetNotificationById), new { id = notificationId }, notificationId);
    }

    /// <summary>
    /// Retrieves a notification by its ID
    /// </summary>
    /// <param name="id">Notification ID</param>
    /// <returns>Notification data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationDto>> GetNotificationById(int id)
    {
        var query = new GetNotificationByIdQuery(id);
        var notification = await Mediator(query);
        return Ok(notification);
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