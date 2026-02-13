﻿namespace AdvancedDevTP.Infrastructure.Entities;

/// <summary>
/// Entité de persistence pour un article de commande en base de données.
/// </summary>
public class OrderItemEntity
{
    /// <summary>
    /// Identifiant unique de l'article de commande.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Identifiant de la commande parente.
    /// </summary>
    public Guid OrderId { get; set; }
    
    /// <summary>
    /// Identifiant du produit commandé.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Nom du produit au moment de la commande.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    
    /// <summary>
    /// Prix unitaire du produit au moment de la commande.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Quantité commandée du produit.
    /// </summary>
    public int Quantity { get; set; }
}