using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookPetGroomingAPI.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for User entity operations
    /// </summary>
    /// <remarks>
    /// Constructor for UserRepository
    /// </remarks>
    /// <param name="context">Database context</param>
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User if found, null otherwise</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>Collection of all users</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves all active users
        /// </summary>
        /// <returns>Collection of active users</returns>
        public async Task<IEnumerable<User>> GetActiveAsync()
        {
            return await context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns>ID of the newly created user</returns>
        public async Task<int> AddAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user.UserId;
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="user">User with updated information</param>
        public async Task UpdateAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user by ID
        /// </summary>
        /// <param name="id">ID of the user to delete</param>
        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a user with the specified username exists
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public async Task<bool> UserExistsAsync(string username)
        {
            return await context.Users
                .AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// Checks if a user with the specified email exists
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users
                .AnyAsync(u => u.Email == email);
        }
    }
}