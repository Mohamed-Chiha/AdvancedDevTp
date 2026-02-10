using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace AdvancedDevTP.Tests.API.UnitTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepositoryAsync> _mockOrderRepo;
    private readonly Mock<IProductRepositoryAsync> _mockProductRepo;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _mockOrderRepo = new Mock<IOrderRepositoryAsync>();
        _mockProductRepo = new Mock<IProductRepositoryAsync>();
        _service = new OrderService(_mockOrderRepo.Object, _mockProductRepo.Object);
    }

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnOrder()
    {
        var order = new Order("Mohamed");
        order.AddItem(Guid.NewGuid(), "Laptop", 500m, 1);
        _mockOrderRepo.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

        var result = await _service.GetByIdAsync(order.Id);

        result.Should().NotBeNull();
        result.CustomerName.Should().Be("Mohamed");
        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(500m);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockOrderRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Order?)null);

        var act = () => _service.GetByIdAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
        var orders = new List<Order>
        {
            new Order("Client 1"),
            new Order("Client 2")
        };
        _mockOrderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateOrderAndDecreaseStock()
    {
        var product = new Product("Laptop", "desc", 10, 500m, true);
        _mockProductRepo.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var request = new CreateOrderRequest
        {
            CustomerName = "Mohamed",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = product.Id, Quantity = 2 }
            }
        };

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.CustomerName.Should().Be("Mohamed");
        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(1000m);
        product.Stock.Should().Be(8); // 10 - 2

        _mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        _mockProductRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingProduct_ShouldThrow()
    {
        var fakeProductId = Guid.NewGuid();
        _mockProductRepo.Setup(r => r.GetByIdAsync(fakeProductId)).ReturnsAsync((Product?)null);

        var request = new CreateOrderRequest
        {
            CustomerName = "Mohamed",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = fakeProductId, Quantity = 1 }
            }
        };

        var act = () => _service.CreateAsync(request);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WithExistingOrder_ShouldCallDelete()
    {
        var id = Guid.NewGuid();
        _mockOrderRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        await _service.DeleteAsync(id);

        _mockOrderRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingOrder_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockOrderRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);

        var act = () => _service.DeleteAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion
}