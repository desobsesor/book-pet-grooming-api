using BookPetGroomingAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookPetGroomingAPI.Domain.Interfaces
{
    /// <summary>
    /// Repository contract for Breed entity.
    /// </summary>
    public interface IBreedRepository
    {
        /// <summary>
        /// Gets all breeds.
        /// </summary>
        /// <returns>List of Breed.</returns>
        Task<IEnumerable<Breed>> GetAllAsync();

        /// <summary>
        /// Gets a breed by its identifier.
        /// </summary>
        /// <param name="id">Breed identifier.</param>
        /// <returns>Breed entity or null.</returns>
        Task<Breed> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new breed.
        /// </summary>
        /// <param name="breed">Breed entity.</param>
        /// <returns>Task.</returns>
        Task AddAsync(Breed breed);

        /// <summary>
        /// Updates an existing breed.
        /// </summary>
        /// <param name="breed">Breed entity.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(Breed breed);

        /// <summary>
        /// Deletes a breed by its identifier.
        /// </summary>
        /// <param name="id">Breed identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(int id);
    }
}