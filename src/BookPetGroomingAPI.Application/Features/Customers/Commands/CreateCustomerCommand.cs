using MediatR;

namespace BookPetGroomingAPI.Application.Features.Customers.Commands
{
    /// <summary>
    /// Command to create a new customer in the system
    /// </summary>
    /// <param name="firstName">Customer first name</param>
    /// <param name="lastName">last name</param>
    /// <param name="email">Customer email</param>
    /// <param name="phone">Customer phone number</param>
    /// <param name="address">Customer address</param>
    public class CreateCustomerCommand(string firstName, string lastName, string email, string phone, string address) : IRequest<int>
    {
        /// <summary>
        /// Customer name
        /// </summary>
        public string FirstName { get; set; } = firstName;

        /// <summary>
        /// Customer LastName
        /// </summary>
        public string LastName { get; set; } = lastName;

        /// <summary>
        /// Customer email
        /// </summary>
        public string Email { get; set; } = email;

        /// <summary>
        /// Customer phone
        /// </summary>
        public string Phone { get; set; } = phone;

        /// <summary>
        /// Customer address
        /// </summary>
        public string Address { get; set; } = address;
    }
}