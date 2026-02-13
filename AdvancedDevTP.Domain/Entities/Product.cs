using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

/// <summary>
/// Entité de domaine représentant un produit du catalogue avec ses propriétés et règles métier.
/// </summary>
public class Product
{
    /// <summary>
    /// Identifiant unique du produit.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Nom du produit.
    /// </summary>
    public string Name  { get; private set; } = string.Empty;
    
    /// <summary>
    /// Description détaillée du produit.
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    
    /// <summary>
    /// Quantité en stock du produit.
    /// </summary>
    public int Stock { get; private set; } = 0;
    
    /// <summary>
    /// Prix unitaire du produit.
    /// </summary>
    public decimal Price { get; private set; }
    
    /// <summary>
    /// Indique si le produit est actif ou non.
    /// </summary>
    public bool IsActive { get; private set; } //false par defaut
    
    /// <summary>
    /// Identifiant optionnel de la catégorie associée.
    /// </summary>
    public Guid? CategoryId { get; private set; }
    
    private Product() { }

    /// <summary>
    /// Crée une nouvelle instance de produit avec validation des données.
    /// </summary>
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
    
    /// <summary>
    /// Crée une nouvelle instance simplifée de produit pour des cas spécifiques.
    /// </summary>
    public Product(Guid productId, decimal productPrice, bool productIsActive)
    {
        Id = productId;
        Price = productPrice;
        IsActive = productIsActive;
    }
    

    /// <summary>
    /// Met à jour les informations du produit avec validation des données.
    /// </summary>
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
    
    /// <summary>
    /// Modifie le prix du produit avec contrôle d'augmentation maximale de 50%.
    /// </summary>
    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new DomainException("Le prix ne peut pas être négatif.");

        if (Price > 0 && newPrice > Price * 1.5m)
            throw new DomainException("Le prix ne peut pas augmenter de plus de 50% en une seule fois.");

        Price = newPrice;
    }

    /// <summary>
    /// Réduit la quantité en stock du produit.
    /// </summary>
    public void DeacreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");
        if (quantity > Stock)
            throw new DomainException("Stock insuffisant.");

        Stock -= quantity;
    }

    /// <summary>
    /// Augmente la quantité en stock du produit.
    /// </summary>
    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        Stock += quantity;
    }

    /// <summary>
    /// Applique une réduction au produit en fonction d'un pourcentage.
    /// </summary>
    public void ApplyDiscount(decimal percentage) { 
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Le pourcentage doit etre entre 0 et 100");
        var discountAmount = (Price * percentage) / 100;
        Price -= discountAmount;
    }

    /// <summary>
    /// Assigne une catégorie au produit.
    /// </summary>
    public void AssignCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}