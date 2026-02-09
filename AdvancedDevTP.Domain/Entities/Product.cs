using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } //false par defaut

    public Product() { 
        IsActive = true;
    }

    public Product(Guid productId, decimal productPrice, bool productIsActive)
    {
        Id = productId;
        Price = productPrice;
        IsActive = productIsActive;
    }

    public void ChangePrice(decimal newPrice) { 
        if (newPrice <= 0) 
            throw new DomainException("le prix doit etre positif");
        if (!IsActive)
            throw new CannotUnloadAppDomainException("Produit inactif");

        Price = newPrice;
    }

    public void ApplyDiscount(decimal percentage) { 
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Le pourcentage doit etre entre 0 et 100");
        var discountAmount = (Price * percentage) / 100;
        Price -= discountAmount;
    }

}