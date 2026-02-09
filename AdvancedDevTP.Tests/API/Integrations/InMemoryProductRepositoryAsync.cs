using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

public class InMemoryProductRepositoryAsync : IProductRepositoryAsync
{
    private readonly List<Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products.ToList());
    }

    public Task AddAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index >= 0)
            _products[index] = product;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _products.RemoveAll(p => p.Id == id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(_products.Any(p => p.Id == id));
    }

    public Task ChangePriceAsync(Guid id, decimal newPrice)
    {
        throw new NotImplementedException();
    }
}
