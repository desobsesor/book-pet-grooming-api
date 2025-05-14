using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Query to retrieve a list of pets belonging to a specific customer
    /// </summary>
    public class GetPetsByCustomerIdQuery(int customerId) : IRequest<List<PetDto>>
    {
        /// <summary>
        /// The unique identifier of the customer whose pets are being requested
        /// </summary>
        public int CustomerId { get; } = customerId;
    }
}