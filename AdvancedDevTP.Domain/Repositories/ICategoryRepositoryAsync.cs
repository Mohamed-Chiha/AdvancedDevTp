using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

public interface ICategoryRepositoryAsync
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}