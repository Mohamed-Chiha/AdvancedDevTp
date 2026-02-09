using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

public interface IProductRepository
{
    Product? GetById(Guid productId);
    IEnumerable<Product> GetAll();
    void Add(Product product);
    void Update(Product product);
    void Delete(Product product);
    void Save(Product product);
}