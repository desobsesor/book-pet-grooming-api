namespace BookPetGroomingAPI.Application.Features.Customers.Queries;

public class CustomerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    // Add other properties as needed
}