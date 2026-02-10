using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<CategoryDTO>> GetAllAsync();
    Task<CategoryDTO> CreateAsync(CreateCategoryRequest request);
    Task<CategoryDTO> UpdateAsync(Guid id, UpdateCategoryRequest request);
    Task DeleteAsync(Guid id);
}