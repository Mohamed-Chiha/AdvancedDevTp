﻿namespace AdvancedDevTP.Infrastructure.Entities;

/// <summary>
/// Entité de persistence pour une commande en base de données.
/// </summary>
public class OrderEntity
{
    /// <summary>
    /// Identifiant unique de la commande.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom du client qui passe la commande.
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
    public List<OrderItemEntity> Items { get; set; } = new();
}