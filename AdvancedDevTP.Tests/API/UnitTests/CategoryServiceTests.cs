using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace AdvancedDevTP.Tests.API.UnitTests;

/// <summary>
/// Tests unitaires pour le service CategoryService avec mocks des dépôts.
/// </summary>
public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepositoryAsync> _mockRepo;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepositoryAsync>();
        _service = new CategoryService(_mockRepo.Object);
    }

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnCategory()
    {
        var category = new Category("Électronique", "Appareils");
        _mockRepo.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var result = await _service.GetByIdAsync(category.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be("Électronique");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Category?)null);

        var act = () => _service.GetByIdAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        var categories = new List<Category>
        {
            new Category("Électronique", "desc"),
            new Category("Vêtements", "desc")
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnCreatedCategory()
    {
        var request = new CreateCategoryRequest
        {
            Name = "Informatique",
            Description = "Matériel informatique"
        };

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("Informatique");
        result.Id.Should().NotBeEmpty();
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_WithExistingCategory_ShouldReturnUpdated()
    {
        var category = new Category("Old", "Old desc");
        _mockRepo.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var request = new UpdateCategoryRequest
        {
            Name = "New",
            Description = "New desc"
        };

        var result = await _service.UpdateAsync(category.Id, request);

        result.Name.Should().Be("New");
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingCategory_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Category?)null);

        var act = () => _service.UpdateAsync(id, new UpdateCategoryRequest { Name = "Test" });

        await act.Should().ThrowAsync<ApplicationServiceException>();
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WithExistingCategory_ShouldCallDelete()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        await _service.DeleteAsync(id);

        _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingCategory_ShouldThrow()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);

        var act = () => _service.DeleteAsync(id);

        await act.Should().ThrowAsync<ApplicationServiceException>()
                 .WithMessage("*introuvable*");
    }

    #endregion
}