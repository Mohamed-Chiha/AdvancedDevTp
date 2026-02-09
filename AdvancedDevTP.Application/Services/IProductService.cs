using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

public interface IProductService
{
    Task<ProductDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    Task<ProductDTO> CreateAsync(CreateProductRequest request);
    Task<ProductDTO> UpdateAsync(Guid id, UpdateProductRequest request);
    Task DeleteAsync(Guid id);
    Task<ProductDTO> ChangePriceAsync(Guid id, ChangePriceRequest request);
}