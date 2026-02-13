using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

/// <summary>
/// Implémentation en mémoire du dépôt de catégories pour les tests d'intégration.
/// Stocke les catégories dans une liste en mémoire au lieu d'une base de données.
/// </summary>
public class InMemoryCategoryRepositoryAsync : ICategoryRepositoryAsync
{
    private readonly List<Category> _categories = new();

    /// <summary>
    /// Récupère une catégorie par son identifiant.
    /// </summary>
    public Task<Category?> GetByIdAsync(Guid id)
        => Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));

    /// <summary>
    /// Récupère toutes les catégories.
    /// </summary>
    public Task<IEnumerable<Category>> GetAllAsync()
        => Task.FromResult<IEnumerable<Category>>(_categories.ToList());

    /// <summary>
    /// Ajoute une nouvelle catégorie à la collection en mémoire.
    /// </summary>
    public Task AddAsync(Category category)
    {
        _categories.Add(category);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Met à jour une catégorie existante.
    /// </summary>
    public Task UpdateAsync(Category category)
    {
        var index = _categories.FindIndex(c => c.Id == category.Id);
        if (index >= 0) _categories[index] = category;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Supprime une catégorie par son identifiant.
    /// </summary>
    public Task DeleteAsync(Guid id)
    {
        _categories.RemoveAll(c => c.Id == id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Vérifie l'existence d'une catégorie par son identifiant.
    /// </summary>
    public Task<bool> ExistsAsync(Guid id)
        => Task.FromResult(_categories.Any(c => c.Id == id));
}