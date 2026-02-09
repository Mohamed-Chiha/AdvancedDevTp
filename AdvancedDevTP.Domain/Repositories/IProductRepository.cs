using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

public interface IProductRepository
{
    Product? GetById(Guid productId);
    void Save(Product product);
}