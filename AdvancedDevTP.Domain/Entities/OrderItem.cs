namespace AdvancedDevTP.Domain.Entities;

/// <summary>
/// Entité de domaine représentant un article d'une commande.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Identifiant unique de l'article de commande.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Identifiant du produit commandé.
    /// </summary>
    public Guid ProductId { get; private set; }
    
    /// <summary>
    /// Nom du produit au moment de la commande.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Prix unitaire du produit au moment de la commande.
    /// </summary>
    public decimal UnitPrice { get; private set; }
    
    /// <summary>
    /// Quantité commandée du produit.
    /// </summary>
    public int Quantity { get; private set; }

    private OrderItem() { }

    /// <summary>
    /// Crée un nouvel article de commande avec les informations du produit.
    /// </summary>
    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}