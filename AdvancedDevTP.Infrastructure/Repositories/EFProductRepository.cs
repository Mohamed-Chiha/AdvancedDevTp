using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Entities;

namespace AdvancedDevTP.Infrastructure.Repositories;

public class EFProductRepository : IProductRepository
{
    public Product GetById(Guid id)
    {
        ProductEntity product = new() { Id = id, Price = 100, IsActive = true };
        var domainProduct = new Product(product.Id, product.Price, product.IsActive);

        return domainProduct;
    }

    public void Save(Product product)
    {
        throw new NotImplementedException();
    }
}