using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Commands
{
    /// <summary>
    /// Handler for processing the CreateGroomerCommand and creating a new groomer
    /// </summary>
    public class CreateGroomerCommandHandler : IRequestHandler<CreateGroomerCommand, int>
    {
        private readonly IGroomerRepository _groomerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for CreateGroomerCommandHandler
        /// </summary>
        /// <param name="groomerRepository">Repository for groomer operations</param>
        /// <param name="mapper">Mapping service</param>
        public CreateGroomerCommandHandler(IGroomerRepository groomerRepository, IMapper mapper)
        {
            _groomerRepository = groomerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to create a new groomer
        /// </summary>
        /// <param name="request">The CreateGroomerCommand with groomer data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created groomer</returns>
        public async Task<int> Handle(CreateGroomerCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new ArgumentException("First name is required", nameof(request.FirstName));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new groomer entity using the domain constructor
                // This approach respects the domain model's encapsulation and validation
                var groomer = new Groomer(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: request.Email,
                    phone: request.Phone,
                    specialization: request.Specialization,
                    yearsOfExperience: request.YearsOfExperience
                );

                // Add the groomer to the repository
                var groomerId = await _groomerRepository.AddAsync(groomer);

                return groomerId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping groomer data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating groomer", ex);
            }
        }
    }
}