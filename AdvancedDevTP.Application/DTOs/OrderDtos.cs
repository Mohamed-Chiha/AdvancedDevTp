using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Requête pour la création d'une nouvelle commande.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Nom du client qui passe la commande.
    /// </summary>
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Liste des articles à commander.
    /// </summary>
    public List<OrderItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Représente un article dans une requête de commande.
/// </summary>
public class OrderItemRequest
{
    /// <summary>
    /// Identifiant du produit à commander.
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Quantité du produit à commander.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

/// <summary>
/// Objet de transfert de données représentant une commande client.
/// </summary>
public class OrderDTO
{
    /// <summary>
    /// Identifiant unique de la commande.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom du client.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date de la commande.
    /// </summary>
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Articles de la commande.
    /// </summary>
    public List<OrderItemDTO> Items { get; set; } = new();
}

/// <summary>
/// Représente un article dans une commande transférée.
/// </summary>
public class OrderItemDTO
{
    /// <summary>
    /// Identifiant du produit commandé.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Nom du produit commandé.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    
    /// <summary>
    /// Prix unitaire du produit au moment de la commande.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Quantité commandée.
    /// </summary>
    public int Quantity { get; set; }
}