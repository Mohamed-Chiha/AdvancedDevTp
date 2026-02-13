using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces; 
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using Moq;
using FluentAssertions;
using Xunit;

namespace AdvancedDevTP.Tests.API.UnitTests;

/// <summary>
/// Tests unitaires pour le service ProductService avec mocks des dépôts.
/// </summary>
public class ProductServiceTests
{
    private readonly Mock<IProductRepositoryAsync> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        // MockBehavior.Strict ensures that if a method is called without a Setup, 
        // the test fails immediately with a MockException (easier to debug).
        _mockRepo = new Mock<IProductRepositoryAsync>();
        _service = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product("Laptop", "Description", 10, (decimal)1299.99, true);
        
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be((decimal)1299.99);
        
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
            new Product("Laptop", "desc", 10, (decimal)1299.99, true),
            new Product("Mouse", "desc", 100, (decimal)29, true)
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
            Price = (decimal)79.99,
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
        var product = new Product("Laptop", "desc", 10, (decimal)899.99, true);
    
        // ADD THESE TWO LINES:
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var request = new UpdateProductRequest
        {
            Name = "Laptop Pro",
            Description = "Updated desc",
            Price = (decimal)1099,
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
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var result = await _service.ChangePriceAsync(product.Id, new ChangePriceRequest { Price = (decimal)130 });

        result.Price.Should().Be((decimal)130);
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