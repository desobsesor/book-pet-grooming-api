using BookPetGroomingAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookPetGroomingAPI.Domain.Interfaces
{
    /// <summary>
    /// Repository contract for PetCategory entity.
    /// </summary>
    public interface IPetCategoryRepository
    {
        /// <summary>
        /// Gets all pet categories.
        /// </summary>
        /// <returns>List of PetCategory.</returns>
        Task<IEnumerable<PetCategory>> GetAllAsync();

        /// <summary>
        /// Gets a pet category by its identifier.
        /// </summary>
        /// <param name="id">PetCategory identifier.</param>
        /// <returns>PetCategory entity or null.</returns>
        Task<PetCategory> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new pet category.
        /// </summary>
        /// <param name="petCategory">PetCategory entity.</param>
        /// <returns>Task.</returns>
        Task AddAsync(PetCategory petCategory);

        /// <summary>
        /// Updates an existing pet category.
        /// </summary>
        /// <param name="petCategory">PetCategory entity.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(PetCategory petCategory);

        /// <summary>
        /// Deletes a pet category by its identifier.
        /// </summary>
        /// <param name="id">PetCategory identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(int id);
    }
}