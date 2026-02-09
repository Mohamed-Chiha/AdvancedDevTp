using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using Moq;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepositoryAsync> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IProductRepositoryAsync>();
        _service = new ProductService(_mockRepo.Object);
    }

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        var product = new Product("Laptop", "desc", 10, 999m, true);
        _mockRepo.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await _service.GetByIdAsync(product.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(999m);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrowApplicationServiceException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var act = () => _service.GetByIdAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new Product("Laptop", "desc", 10, 999m, true),
            new Product("Mouse", "desc", 100, 29m, true)
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnCreatedProduct()
    {
        var request = new CreateProductRequest
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard",
            Price = 79.99m,
            Stock = 50
        };

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("Keyboard");
        result.Price.Should().Be(79.99m);
        result.Id.Should().NotBeEmpty();

        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_WithExistingProduct_ShouldReturnUpdatedProduct()
    {
        var product = new Product("Laptop", "desc", 10, 999m, true);
        _mockRepo.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var request = new UpdateProductRequest
        {
            Name = "Laptop Pro",
            Description = "Updated desc",
            Price = 1099m,
            Stock = 5
        };

        var result = await _service.UpdateAsync(product.Id, request);

        result.Name.Should().Be("Laptop Pro");
        result.Price.Should().Be(1099m);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingProduct_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var act = () => _service.UpdateAsync(id, new UpdateProductRequest
        {
            Name = "Test", Price = 10, Stock = 1
        });

        await act.Should().ThrowAsync<ApplicationServiceException>();
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WithExistingProduct_ShouldCallRepositoryDelete()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        await _service.DeleteAsync(id);

        _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingProduct_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);

        var act = () => _service.DeleteAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion

    #region ChangePriceAsync

    [Fact]
    public async Task ChangePriceAsync_WithValidIncrease_ShouldReturnUpdatedProduct()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);
        _mockRepo.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await _service.ChangePriceAsync(product.Id, new ChangePriceRequest { Price = 130m });

        result.Price.Should().Be(130m);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task ChangePriceAsync_WithNonExistingProduct_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var act = () => _service.ChangePriceAsync(id, new ChangePriceRequest { Price = 50m });

        await act.Should().ThrowAsync<ApplicationServiceException>();
    }

    #endregion
}