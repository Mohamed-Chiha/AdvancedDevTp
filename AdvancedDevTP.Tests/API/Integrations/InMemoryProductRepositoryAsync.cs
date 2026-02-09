using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

public class InMemoryProductRepositoryAsync : IProductRepositoryAsync
{
    private readonly Dictionary<Guid, Product> _store = new();

    public Task<Product?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_store.TryGetValue(id, out var p) ? p : null);
    }

    public Task SaveAsync(Product product)
    {
        _store[product.Id] = product;
        return Task.CompletedTask;
    }

    // Helper pour initialiser le test
    public void Seed(Product product)
    {
        _store[product.Id] = product;
    }
}