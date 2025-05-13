using BookPetGroomingAPI.Domain.Entities;

namespace BookPetGroomingAPI.Domain.Interfaces
{
    /// <summary>
    /// Repository contract for Notification entity.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns>List of Notification.</returns>
        Task<IEnumerable<Notification>> GetAllAsync();

        /// <summary>
        /// Gets a notification by its identifier.
        /// </summary>
        /// <param name="id">Notification identifier.</param>
        /// <returns>Notification entity or null.</returns>
        Task<Notification> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new notification.
        /// </summary>
        /// <param name="notification">Notification entity.</param>
        /// <returns>Task.</returns>
        Task AddAsync(Notification notification);

        /// <summary>
        /// Updates an existing notification.
        /// </summary>
        /// <param name="notification">Notification entity.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(Notification notification);

        /// <summary>
        /// Deletes a notification by its identifier.
        /// </summary>
        /// <param name="id">Notification identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(int id);
    }
}