namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Objet de transfert de données représentant un produit du catalogue.
/// </summary>
public class ProductDTO
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
}