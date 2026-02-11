using System.Net;
using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

public class ProductService : IProductService
{
    private readonly IProductRepositoryAsync _productRepository;
    public ProductService(IProductRepositoryAsync productRepository)
    {
        _productRepository = productRepository;
    }
    
    public void ChangeProductPrice(Guid productId, decimal newPrice)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDTO> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new ApplicationServiceException($"Produit avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
        return MapToProductResponseDTO(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToProductResponseDTO);
    }

    public async Task<ProductDTO> CreateAsync(CreateProductRequest request)
    {
        var product = new Product(request.Name, request.Description, request.Stock, request.Price, isActive: true);
        await _productRepository.AddAsync(product);
        return MapToProductResponseDTO(product);
    }

    public async Task<ProductDTO> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new ApplicationServiceException($"Produit avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
        product.Update(request.Name, request.Description, request.Stock, request.Price,isActive: false);
        await _productRepository.UpdateAsync(product);
        return MapToProductResponseDTO(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
            throw new ApplicationServiceException($"Produit avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);

        await _productRepository.DeleteAsync(id);
    }

    public async Task<ProductDTO> ChangePriceAsync(Guid id, ChangePriceRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new ApplicationServiceException($"Produit avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);

        product.ChangePrice(request.Price);
        await _productRepository.UpdateAsync(product);
        return MapToProductResponseDTO(product);
    }
    
    /// <summary>
    /// Map un Product vers un ProductDTO. En cas de mapping plus complexe, on pourrait utiliser AutoMapper ou un outil similaire.               
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    private static ProductDTO MapToProductResponseDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive =  product.IsActive
        };
    }
}