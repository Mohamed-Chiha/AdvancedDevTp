using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

/// <summary>
/// Implémentation en mémoire du dépôt de produits pour les tests d'intégration.
/// Stocke les produits dans une liste en mémoire au lieu d'une base de données.
/// </summary>
public class InMemoryProductRepositoryAsync : IProductRepositoryAsync
{
    private readonly List<Product> _products = new();

    /// <summary>
    /// Ajoute un produit à la collection en mémoire.
    /// </summary>
    public Task AddAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Récupère un produit par son identifiant.
    /// </summary>
    public Task<Product?> GetByIdAsync(Guid id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(p);
    }

    /// <summary>
    /// Récupère tous les produits.
    /// </summary>
    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products);
    }

    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    public Task UpdateAsync(Product product)
    {
        // Dans une liste en mémoire, l'objet est souvent une référence, 
        // donc la modification est parfois automatique, mais pour être sûr :
        var existingIndex = _products.FindIndex(p => p.Id == product.Id);
        if (existingIndex >= 0)
        {
            _products[existingIndex] = product;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Supprime un produit par son identifiant.
    /// </summary>
    public Task DeleteAsync(Guid id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p != null) _products.Remove(p);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Vérifie l'existence d'un produit par son identifiant.
    /// </summary>
    public Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(_products.Any(p => p.Id == id));
    }

    /// <summary>
    /// Change le prix d'un produit.
    /// </summary>
    public Task ChangePriceAsync(Guid id, decimal newPrice)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        
        // CORRECTION ICI : Utiliser ChangePrice au lieu de Update
        if (p != null) 
        {
            p.ChangePrice(newPrice);
        }
        
        return Task.CompletedTask;
    }
}