using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

/// <summary>
/// Interface de dépôt asynchrone pour la gestion des catégories.
/// </summary>
public interface ICategoryRepositoryAsync
{
    /// <summary>
    /// Récupère une catégorie par son identifiant de manière asynchrone.
    /// </summary>
    Task<Category?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère toutes les catégories de manière asynchrone.
    /// </summary>
    Task<IEnumerable<Category>> GetAllAsync();
    
    /// <summary>
    /// Ajoute une nouvelle catégorie de manière asynchrone.
    /// </summary>
    Task AddAsync(Category category);
    
    /// <summary>
    /// Met à jour une catégorie existante de manière asynchrone.
    /// </summary>
    Task UpdateAsync(Category category);
    
    /// <summary>
    /// Supprime une catégorie par son identifiant de manière asynchrone.
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Vérifie l'existence d'une catégorie par son identifiant.
    /// </summary>
    Task<bool> ExistsAsync(Guid id);
}