using System.Net;
using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Application.Interfaces;

/// <summary>
/// Implémentation du service métier pour la gestion des commandes clients.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepositoryAsync _orderRepository;
    private readonly IProductRepositoryAsync _productRepository;

    /// <summary>
    /// Initialise une nouvelle instance du service OrderService.
    /// </summary>
    public OrderService(IOrderRepositoryAsync orderRepository, IProductRepositoryAsync productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Récupère une commande par son identifiant.
    /// </summary>
    public async Task<OrderDTO> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            throw new ApplicationServiceException($"Commande avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
        return MapToDTO(order);
    }

    /// <summary>
    /// Récupère toutes les commandes.
    /// </summary>
    public async Task<IEnumerable<OrderDTO>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDTO);
    }

    /// <summary>
    /// Crée une nouvelle commande avec validation des produits et des stocks.
    /// </summary>
    public async Task<OrderDTO> CreateAsync(CreateOrderRequest request)
    {
        var order = new Order(request.CustomerName);

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product is null)
                throw new ApplicationServiceException($"Produit '{item.ProductId}' introuvable.", HttpStatusCode.NotFound);

            order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
            product.DeacreaseStock(item.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        await _orderRepository.AddAsync(order);
        return MapToDTO(order);
    }

    /// <summary>
    /// Supprime une commande par son identifiant.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        if (!await _orderRepository.ExistsAsync(id))
            throw new ApplicationServiceException($"Commande avec l'id '{id}' introuvable.", HttpStatusCode.NotFound);
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