using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

public interface IProductRepositoryAsync
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task ChangePriceAsync(Guid id, decimal newPrice);
}