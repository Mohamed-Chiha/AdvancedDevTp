using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

public interface IOrderRepositoryAsync
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}