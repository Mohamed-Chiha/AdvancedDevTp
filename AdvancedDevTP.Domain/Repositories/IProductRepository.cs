using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

/// <summary>
/// Interface de dépôt synchrone pour la gestion des produits.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Récupère un produit par son identifiant.
    /// </summary>
    Product? GetById(Guid productId);
    
    /// <summary>
    /// Récupère tous les produits.
    /// </summary>
    IEnumerable<Product> GetAll();
    
    /// <summary>
    /// Ajoute un nouveau produit.
    /// </summary>
    void Add(Product product);
    
    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    void Update(Product product);
    
    /// <summary>
    /// Supprime un produit.
    /// </summary>
    void Delete(Product product);
    
    /// <summary>
    /// Enregistre les modifications d'un produit.
    /// </summary>
    void Save(Product product);
}