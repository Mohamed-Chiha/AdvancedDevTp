using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

/// <summary>
/// Service métier pour la gestion des catégories de produits.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Récupère une catégorie par son identifiant.
    /// </summary>
    Task<CategoryDTO> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère toutes les catégories.
    /// </summary>
    Task<IEnumerable<CategoryDTO>> GetAllAsync();
    
    /// <summary>
    /// Crée une nouvelle catégorie.
    /// </summary>
    Task<CategoryDTO> CreateAsync(CreateCategoryRequest request);
    
    /// <summary>
    /// Met à jour une catégorie existante.
    /// </summary>
    Task<CategoryDTO> UpdateAsync(Guid id, UpdateCategoryRequest request);
    
    /// <summary>
    /// Supprime une catégorie.
    /// </summary>
    Task DeleteAsync(Guid id);
}