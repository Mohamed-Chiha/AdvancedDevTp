namespace AdvancedDevTP.Infrastructure.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemEntity> Items { get; set; } = new();
}