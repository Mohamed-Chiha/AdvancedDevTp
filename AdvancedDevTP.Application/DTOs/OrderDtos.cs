using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

public class CreateOrderRequest
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class OrderDTO
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
}

public class OrderItemDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}