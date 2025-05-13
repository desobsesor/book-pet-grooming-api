using AutoMapper;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Appointments.Commands
{
    /// <summary>
    /// Handler for processing the CreateAppointmentCommand and creating a new appointment
    /// </summary>
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for CreateAppointmentCommandHandler
        /// </summary>
        /// <param name="appointmentRepository">Repository for appointment operations</param>
        /// <param name="mapper">Mapping service</param>
        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to create a new appointment
        /// </summary>
        /// <param name="request">The CreateAppointmentCommand with appointment data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created appointment</returns>
        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Status))
            {
                throw new ArgumentException("Status is required", nameof(request.Status));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Create a new appointment entity using the domain constructor
                // This approach respects the domain model's encapsulation and validation
                var appointment = new Appointment(request.PetId, request.GroomerId, request.AppointmentDate, request.StartTime, request.Status, request.Notes, request.Price);

                // Add the appointment to the repository
                var appointmentId = await _appointmentRepository.AddAsync(appointment);

                return appointmentId;
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle mapping errors specifically
                throw new ApplicationException("Error mapping appointment data", ex);
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not OperationCanceledException)
            {
                // Handle other exceptions
                throw new ApplicationException("Error creating appointment", ex);
            }
        }
    }
}