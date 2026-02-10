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
    public Guid? CategoryId { get; private set; }
    
    private Product() { }


    public Product(string name, string description, int stock, decimal price, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom est obligatoire.");
        if (name.Length > 200)
            throw new DomainException("Le nom ne peut pas dépasser 200 caractères.");
        if (price < 0)
            throw new DomainException("Le prix ne peut pas être négatif.");
        if (stock < 0)
            throw new DomainException("Le stock ne peut pas être négatif.");

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
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Le nom est obligatoire.");
        if (newName.Length > 200)
            throw new DomainException("Le nom ne peut pas dépasser 200 caractères.");
        if (newPrice < 0)
            throw new DomainException("Le prix ne peut pas être négatif.");
        if (newStock < 0)
            throw new DomainException("Le stock ne peut pas être négatif.");

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
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");
        if (quantity > Stock)
            throw new DomainException("Stock insuffisant.");

        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        Stock += quantity;
    }

    public void ApplyDiscount(decimal percentage) { 
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Le pourcentage doit etre entre 0 et 100");
        var discountAmount = (Price * percentage) / 100;
        Price -= discountAmount;
    }

    public void AssignCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}