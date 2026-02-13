using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

/// <summary>
/// Interface de dépôt asynchrone pour la gestion des produits.
/// </summary>
public interface IProductRepositoryAsync
{
    /// <summary>
    /// Récupère un produit par son identifiant de manière asynchrone.
    /// </summary>
    Task<Product?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère tous les produits de manière asynchrone.
    /// </summary>
    Task<IEnumerable<Product>> GetAllAsync();
    
    /// <summary>
    /// Ajoute un nouveau produit de manière asynchrone.
    /// </summary>
    Task AddAsync(Product product);
    
    /// <summary>
    /// Met à jour un produit existant de manière asynchrone.
    /// </summary>
    Task UpdateAsync(Product product);
    
    /// <summary>
    /// Supprime un produit par son identifiant de manière asynchrone.
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Vérifie l'existence d'un produit par son identifiant.
    /// </summary>
    Task<bool> ExistsAsync(Guid id);
    
    /// <summary>
    /// Change le prix d'un produit de manière asynchrone.
    /// </summary>
    Task ChangePriceAsync(Guid id, decimal newPrice);
}