using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderDTO>> GetAllAsync();
    Task<OrderDTO> CreateAsync(CreateOrderRequest request);
    Task DeleteAsync(Guid id);
}