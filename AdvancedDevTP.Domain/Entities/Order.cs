using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

/// <summary>
/// Entité de domaine représentant une commande client avec ses articles.
/// </summary>
public class Order
{
    /// <summary>
    /// Identifiant unique de la commande.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Date de création de la commande.
    /// </summary>
    public DateTime OrderDate { get; private set; }
    
    /// <summary>
    /// Nom du client qui passe la commande.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    private readonly List<OrderItem> _items = new();
    
    /// <summary>
    /// Collection en lecture seule des articles de la commande.
    /// </summary>
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    /// <summary>
    /// Crée une nouvelle commande pour un client avec validation du nom.
    /// </summary>
    public Order(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Le nom du client est obligatoire.");

        Id = Guid.NewGuid();
        CustomerName = customerName;
        OrderDate = DateTime.UtcNow;
        TotalAmount = 0;
    }

    /// <summary>
    /// Ajoute un article à la commande avec validation des données.
    /// </summary>
    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        if (unitPrice < 0)
            throw new DomainException("Le prix unitaire ne peut pas être négatif.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem is not null)
            throw new DomainException("Ce produit est déjà dans la commande.");

        _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
        RecalculateTotal();
    }

    /// <summary>
    /// Supprime un article de la commande par son identifiant de produit.
    /// </summary>
    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            throw new DomainException("Produit non trouvé dans la commande.");

        _items.Remove(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.UnitPrice * i.Quantity);
    }
}