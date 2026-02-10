using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

public class OrderService : IOrderService
{
    private readonly IOrderRepositoryAsync _orderRepository;
    private readonly IProductRepositoryAsync _productRepository;

    public OrderService(IOrderRepositoryAsync orderRepository, IProductRepositoryAsync productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDTO> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            throw new ApplicationServiceException($"Commande avec l'id '{id}' introuvable.");
        return MapToDTO(order);
    }

    public async Task<IEnumerable<OrderDTO>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDTO);
    }

    public async Task<OrderDTO> CreateAsync(CreateOrderRequest request)
    {
        var order = new Order(request.CustomerName);

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product is null)
                throw new ApplicationServiceException($"Produit '{item.ProductId}' introuvable.");

            order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
            product.DeacreaseStock(item.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        await _orderRepository.AddAsync(order);
        return MapToDTO(order);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _orderRepository.ExistsAsync(id))
            throw new ApplicationServiceException($"Commande avec l'id '{id}' introuvable.");
        await _orderRepository.DeleteAsync(id);
    }

    private static OrderDTO MapToDTO(Order order) => new()
    {
        Id = order.Id,
        CustomerName = order.CustomerName,
        OrderDate = order.OrderDate,
        TotalAmount = order.TotalAmount,
        Items = order.Items.Select(i => new OrderItemDTO
        {
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        }).ToList()
    };
}