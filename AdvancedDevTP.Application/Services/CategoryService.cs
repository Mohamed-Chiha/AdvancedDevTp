using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepositoryAsync _repository;

    public CategoryService(ICategoryRepositoryAsync repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDTO> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.");
        return MapToDTO(category);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(MapToDTO);
    }

    public async Task<CategoryDTO> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category(request.Name, request.Description);
        await _repository.AddAsync(category);
        return MapToDTO(category);
    }

    public async Task<CategoryDTO> UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.");

        category.Update(request.Name, request.Description);
        await _repository.UpdateAsync(category);
        return MapToDTO(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new ApplicationServiceException($"Catégorie avec l'id '{id}' introuvable.");
        await _repository.DeleteAsync(id);
    }

    private static CategoryDTO MapToDTO(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };
}