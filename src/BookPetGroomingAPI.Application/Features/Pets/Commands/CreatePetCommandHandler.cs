using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Commands
{
    /// <summary>
    /// Handler for processing the CreatePetCommand and creating a new pet
    /// </summary>
    public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, int>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for CreatePetCommandHandler
        /// </summary>
        /// <param name="petRepository">Repository for pet operations</param>
        /// <param name="mapper">Mapping service</param>
        public CreatePetCommandHandler(IPetRepository petRepository, IMapper mapper)
        {
            _petRepository = petRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to create a new pet
        /// </summary>
        /// <param name="request">The CreatePetCommand with pet data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created pet</returns>
        public async Task<int> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required", nameof(request.Name));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new pet entity using the domain constructor
                // This approach respects the domain model's encapsulation and validation
                var pet = new Pet(request.Name, request.Weight, request.DateOfBirth, request.Gender, request.CustomerId, request.BreedId, request.CategoryId);

                // Add the pet to the repository
                var petId = await _petRepository.AddAsync(pet);

                return petId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping pet data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating pet", ex);
            }
        }
    }
}