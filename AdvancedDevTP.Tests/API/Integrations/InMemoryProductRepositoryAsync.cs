using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

public class InMemoryProductRepositoryAsync : IProductRepositoryAsync
{
    private readonly List<Product> _products = new();

    public Task AddAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(p);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products);
    }

    public Task UpdateAsync(Product product)
    {
        // Dans une liste en mémoire, l'objet est souvent une référence, 
        // donc la modification est parfois automatique, mais pour être sûr :
        var existingIndex = _products.FindIndex(p => p.Id == product.Id);
        if (existingIndex >= 0)
        {
            _products[existingIndex] = product;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p != null) _products.Remove(p);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(_products.Any(p => p.Id == id));
    }

    public Task ChangePriceAsync(Guid id, decimal newPrice)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        
        // CORRECTION ICI : Utiliser ChangePrice au lieu de Update
        if (p != null) 
        {
            p.ChangePrice(newPrice);
        }
        
        return Task.CompletedTask;
    }
}