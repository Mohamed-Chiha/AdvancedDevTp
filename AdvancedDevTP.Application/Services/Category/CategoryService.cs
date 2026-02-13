using System.Net;
using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

/// <summary>
/// Implémentation du service métier pour la gestion des catégories de produits.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepositoryAsync _repository;

    /// <summary>
    /// Initialise une nouvelle instance du service CategoryService.
    /// </summary>
    public CategoryService(ICategoryRepositoryAsync repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Récupère une catégorie par son identifiant.
    /// </summary>
    public async Task<CategoryDTO> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
        return MapToDTO(category);
    }

    /// <summary>
    /// Récupère toutes les catégories.
    /// </summary>
    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(MapToDTO);
    }

    /// <summary>
    /// Crée une nouvelle catégorie.
    /// </summary>
    public async Task<CategoryDTO> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category(request.Name, request.Description);
        await _repository.AddAsync(category);
        return MapToDTO(category);
    }

    /// <summary>
    /// Met à jour une catégorie existante.
    /// </summary>
    public async Task<CategoryDTO> UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);

        category.Update(request.Name, request.Description);
        await _repository.UpdateAsync(category);
        return MapToDTO(category);
    }

    /// <summary>
    /// Supprime une catégorie.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
        await _repository.DeleteAsync(id);
    }

    private static CategoryDTO MapToDTO(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };
}