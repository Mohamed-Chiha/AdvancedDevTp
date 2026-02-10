using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

public class InMemoryOrderRepositoryAsync : IOrderRepositoryAsync
{
    private readonly List<Order> _orders = new();

    public Task<Order?> GetByIdAsync(Guid id)
        => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    public Task<IEnumerable<Order>> GetAllAsync()
        => Task.FromResult<IEnumerable<Order>>(_orders.ToList());

    public Task AddAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _orders.RemoveAll(o => o.Id == id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
        => Task.FromResult(_orders.Any(o => o.Id == id));
}