using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name  { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Stock { get; private set; } = 0;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } //false par defaut
    
    private Product() { }


    public Product(string name, string description, int stock, decimal price, bool isActive)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Stock = stock;
        Price = price;          
        IsActive = isActive;    
    }
    
    public Product(Guid productId, decimal productPrice, bool productIsActive)
    {
        Id = productId;
        Price = productPrice;
        IsActive = productIsActive;
    }
    

    public void Update(string newName, string newDescription, int newStock, decimal newPrice, bool isActive)
    {
        Name = newName;
        Description = newDescription;
        Stock = newStock;
        Price = newPrice;
        IsActive = isActive;
    }
    
    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new DomainException("Le prix ne peut pas être négatif.");

        if (Price > 0 && newPrice > Price * 1.5m)
            throw new DomainException("Le prix ne peut pas augmenter de plus de 50% en une seule fois.");

        Price = newPrice;
    }

    public void DeacreaseStock(int quantity)
    {
        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
    }

    public void ApplyDiscount(decimal percentage) { 
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Le pourcentage doit etre entre 0 et 100");
        var discountAmount = (Price * percentage) / 100;
        Price -= discountAmount;
    }

}