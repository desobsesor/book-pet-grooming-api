using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Commands
{
    /// <summary>
    /// Handler for processing the CreateBreedCommand and creating a new breed
    /// </summary>
    /// <remarks>
    /// Constructor for CreateBreedCommandHandler
    /// </remarks>
    /// <param name="breedRepository">Repository for breed operations</param>
    /// <param name="mapper">Mapping service</param>
    public class CreateBreedCommandHandler(IBreedRepository breedRepository, IMapper mapper) : IRequestHandler<CreateBreedCommand, int>
    {

        /// <summary>
        /// Handles the command to create a new breed
        /// </summary>
        /// <param name="request">The CreateBreedCommand with breed data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created breed</returns>
        public async Task<int> Handle(CreateBreedCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required", nameof(request.Name));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new breed entity using the domain constructor
                var breed = new Breed(request.Name, request.Species, request.CoatType, request.GroomingDifficulty);

                // Add the breed to the repository
                var breedId = await breedRepository.AddAsync(breed);

                return breedId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping breed data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating breed", ex);
            }
        }
    }
}