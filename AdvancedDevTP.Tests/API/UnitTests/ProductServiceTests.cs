using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces; // Namespace where ProductService is located
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using Moq;
using FluentAssertions;
using Xunit;

namespace AdvancedDevTP.Tests.API.UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepositoryAsync> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        // MockBehavior.Strict ensures that if a method is called without a Setup, 
        // the test fails immediately with a MockException (easier to debug).
        _mockRepo = new Mock<IProductRepositoryAsync>(MockBehavior.Strict);
        _service = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product("Laptop", "Description", 10, 999m, true);
        
        // SETUP: We tell the Mock to return 'product' for ANY Guid provided.
        // This is crucial because 'product.Id' is generated randomly inside the constructor.
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(999m);
        
        // Verification (Optional but good)
        _mockRepo.Verify(r => r.GetByIdAsync(product.Id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrowApplicationServiceException()
    {
        var id = Guid.NewGuid();
        
        // We explicitly simulate a "Not Found" scenario by returning null
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Product?)null);

        // Act
        var act = () => _service.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

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

        Product? capturedProduct = null;

        // 1. Setup AddAsync to run successfully and capture the product
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>()))
                 .Callback<Product>(p => capturedProduct = p)
                 .Returns(Task.CompletedTask);

        // 2. Setup GetByIdAsync to return the CAPTURED product
        // Use deferral (() => capturedProduct) to return the value captured during AddAsync
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(() => capturedProduct);

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("Keyboard");
        result.Id.Should().NotBeEmpty();
        
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingProduct_ShouldReturnUpdatedProduct()
    {
        var product = new Product("Laptop", "desc", 10, 999m, true);
        
        // Setup GetById to find the product
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
        
        // Setup Update to succeed
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var request = new UpdateProductRequest
        {
            Name = "Laptop Pro",
            Description = "Updated desc",
            Price = 1099m,
            Stock = 5
        };

        var result = await _service.UpdateAsync(product.Id, request);

        result.Name.Should().Be("Laptop Pro");
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingProduct_ShouldCallRepositoryDelete()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);
        _mockRepo.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(id);

        _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingProduct_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);

        var act = () => _service.DeleteAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>();
    }

    [Fact]
    public async Task ChangePriceAsync_WithValidIncrease_ShouldReturnUpdatedProduct()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var result = await _service.ChangePriceAsync(product.Id, new ChangePriceRequest { Price = 130m });

        result.Price.Should().Be(130m);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }
    
    // Add missing mocks for tests that might call other methods (like UpdateAsync calling Exists internally if implemented differently)
    [Fact]
    public async Task UpdateAsync_WithNonExistingProduct_ShouldThrow()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);
        
        var act = () => _service.UpdateAsync(Guid.NewGuid(), new UpdateProductRequest { Name = "Test", Price = 10 });
        
        await act.Should().ThrowAsync<ApplicationServiceException>();
    }
    
    [Fact]
    public async Task ChangePriceAsync_WithNonExistingProduct_ShouldThrow()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);
        
        var act = () => _service.ChangePriceAsync(Guid.NewGuid(), new ChangePriceRequest { Price = 50 });
        
        await act.Should().ThrowAsync<ApplicationServiceException>();
    }
}