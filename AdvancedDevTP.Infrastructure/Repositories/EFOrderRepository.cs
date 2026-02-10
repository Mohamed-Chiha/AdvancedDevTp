using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Data;
using AdvancedDevTP.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevTP.Infrastructure.Repositories;

public class EFOrderRepository : IOrderRepositoryAsync
{
    private readonly AppDbContext _context;

    public EFOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        var entities = await _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(Order order)
    {
        var entity = MapToEntity(order);
        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (entity is null) return;

        _context.OrderItems.RemoveRange(entity.Items);
        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id);
    }

    #region Mapping

    private static Order MapToDomain(OrderEntity entity)
    {
        var order = (Order)System.Runtime.Serialization.FormatterServices
            .GetUninitializedObject(typeof(Order));

        var type = typeof(Order);
        type.GetProperty(nameof(Order.Id))!.SetValue(order, entity.Id);
        type.GetProperty(nameof(Order.CustomerName))!.SetValue(order, entity.CustomerName);
        type.GetProperty(nameof(Order.OrderDate))!.SetValue(order, entity.OrderDate);
        type.GetProperty(nameof(Order.TotalAmount))!.SetValue(order, entity.TotalAmount);

        // Map items into the private _items list
        var itemsField = typeof(Order).GetField("_items",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var items = entity.Items.Select(MapItemToDomain).ToList();
        itemsField!.SetValue(order, items);

        return order;
    }

    private static OrderItem MapItemToDomain(OrderItemEntity entity)
    {
        var item = (OrderItem)System.Runtime.Serialization.FormatterServices
            .GetUninitializedObject(typeof(OrderItem));

        var type = typeof(OrderItem);
        type.GetProperty(nameof(OrderItem.Id))!.SetValue(item, entity.Id);
        type.GetProperty(nameof(OrderItem.ProductId))!.SetValue(item, entity.ProductId);
        type.GetProperty(nameof(OrderItem.ProductName))!.SetValue(item, entity.ProductName);
        type.GetProperty(nameof(OrderItem.UnitPrice))!.SetValue(item, entity.UnitPrice);
        type.GetProperty(nameof(OrderItem.Quantity))!.SetValue(item, entity.Quantity);

        return item;
    }

    private static OrderEntity MapToEntity(Order order)
    {
        return new OrderEntity
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(i => new OrderItemEntity
            {
                Id = i.Id,
                OrderId = order.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    #endregion
}