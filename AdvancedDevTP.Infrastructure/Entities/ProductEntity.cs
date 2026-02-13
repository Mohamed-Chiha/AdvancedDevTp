﻿namespace AdvancedDevTP.Infrastructure.Entities;

/// <summary>
/// Entité de persistence pour un produit en base de données.
/// </summary>
public class ProductEntity
{
    /// <summary>
    /// Identifiant unique du produit.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom du produit.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description du produit.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Prix unitaire du produit.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Quantité en stock du produit.
    /// </summary>
    public int Stock { get; set; }
    
    /// <summary>
    /// Indique si le produit est actif.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Identifiant optionnel de la catégorie associée.
    /// </summary>
    public Guid? CategoryId { get; set; }
}