using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

/// <summary>
/// Service métier pour la gestion des produits du catalogue.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Récupère un produit par son identifiant.
    /// </summary>
    Task<ProductDTO> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère tous les produits.
    /// </summary>
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    
    /// <summary>
    /// Crée un nouveau produit.
    /// </summary>
    Task<ProductDTO> CreateAsync(CreateProductRequest request);
    
    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    Task<ProductDTO> UpdateAsync(Guid id, UpdateProductRequest request);
    
    /// <summary>
    /// Supprime un produit.
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Change le prix d'un produit.
    /// </summary>
    Task<ProductDTO> ChangePriceAsync(Guid id, ChangePriceRequest request);
}