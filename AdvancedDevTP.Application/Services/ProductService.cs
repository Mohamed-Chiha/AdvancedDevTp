using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public void ChangeProductPrice(Guid productId, decimal newPrice)
    {
        var product = _productRepository.GetById(productId) ?? throw new ApplicationException("Produit introuvable");
        product.ChangePrice(newPrice);
        _productRepository.Save(product);
    }
    
}