namespace BookPetGroomingAPI.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime? ModificationDate { get; private set; }

    // Private constructor for EF Core
    private Product() { }

    public Product(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The product name cannot be empty", nameof(name));

        if (price <= 0)
            throw new ArgumentException("The price must be greater than zero", nameof(price));

        if (stock < 0)
            throw new ArgumentException("The stock cannot be negative", nameof(stock));

        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        Active = true;
        CreationDate = DateTime.UtcNow;
    }

    public void UpdateInformation(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The product name cannot be empty", nameof(name));

        if (price <= 0)
            throw new ArgumentException("The price must be greater than zero", nameof(price));

        Name = name;
        Description = description;
        Price = price;
        ModificationDate = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("The quantity must be greater than zero", nameof(quantity));

        Stock += quantity;
        ModificationDate = DateTime.UtcNow;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("The quantity must be greater than zero", nameof(quantity));

        if (quantity > Stock)
            throw new InvalidOperationException("There is not enough stock available");

        Stock -= quantity;
        ModificationDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        Active = true;
        ModificationDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Active = false;
        ModificationDate = DateTime.UtcNow;
    }
}