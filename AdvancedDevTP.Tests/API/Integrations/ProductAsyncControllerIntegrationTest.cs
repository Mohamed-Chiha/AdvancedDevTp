using System.Net;
using System.Net.Http.Json;
using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AdvancedDevTP.Tests.API.Integrations;

public class ProductAsyncControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly InMemoryProductRepositoryAsync _repo;

    public ProductAsyncControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _repo = (InMemoryProductRepositoryAsync)factory.Services.GetRequiredService<IProductRepositoryAsync>();
    }

    [Fact]
    public async Task ChangePrice_Should_Return_NoContent_And_Save_Product()
    {
        // Arrange
        var product = new Product();
        _repo.Seed(product);

        var request = new ChangePriceRequest { Price = 20 };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/productasync/{product.Id}/price",
            request
        );

        // Assert - HTTP
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updated = await _repo.GetByIdAsync(product.Id);
        Assert.Equal(20, updated!.Price);
    }
}