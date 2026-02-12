using System.Net;
using System.Net.Http.Json;
using AdvancedDevTP.Application.DTOs;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.Integrations;

public class ProductAsyncControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductAsyncControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region GET

    [Fact]
    public async Task GetAll_WhenEmpty_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync("/api/Product");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
        products.Should().NotBeNull();
    }

    #endregion

    #region POST + GET

    [Fact]
    public async Task Create_WithValidData_ShouldReturn201AndProduct()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Clavier Mécanique",
            Description = "Switch Cherry MX Blue",
            Price = (decimal)89.99,
            Stock = 30
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<ProductDTO>();
        created.Should().NotBeNull();
        created!.Name.Should().Be("Clavier Mécanique");
        created.Price.Should().Be((decimal)89.99);
        created.Id.Should().NotBeEmpty();

        // Verify location header
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ThenGetById_ShouldReturnSameProduct()
    {
        // Create
        var request = new CreateProductRequest
        {
            Name = "Souris Gamer",
            Description = "16000 DPI",
            Price = (decimal)59.99,
            Stock = 50
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Product", request);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDTO>();

        // Get by ID
        var getResponse = await _client.GetAsync($"/api/Product/{created!.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrieved = await getResponse.Content.ReadFromJsonAsync<ProductDTO>();
        retrieved!.Id.Should().Be(created.Id);
        retrieved.Name.Should().Be("Souris Gamer");
    }

    [Fact]
    public async Task Create_WithInvalidData_ShouldReturn400()
    {
        var request = new CreateProductRequest
        {
            Name = "", // Invalid: empty name
            Price = (decimal)10,
            Stock = 5
        };

        var response = await _client.PostAsJsonAsync("/api/Product", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PUT

    [Fact]
    public async Task Update_WithExistingProduct_ShouldReturn200()
    {
        // Create first
        var createRequest = new CreateProductRequest
        {
            Name = "Écran 24p",
            Description = "Full HD",
            Price = (decimal)199,
            Stock = 15
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Product", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDTO>();

        // Update
        var updateRequest = new UpdateProductRequest
        {
            Name = "Écran 27p 4K",
            Description = "Ultra HD",
            Price = (decimal)349,
            Stock = 10
        };
        var updateResponse = await _client.PutAsJsonAsync($"/api/Product/{created!.Id}", updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await updateResponse.Content.ReadFromJsonAsync<ProductDTO>();
        updated!.Name.Should().Be("Écran 27p 4K");
        updated.Price.Should().Be((decimal)349);
    }

    [Fact]
    public async Task Update_WithNonExistingProduct_ShouldReturn404()
    {
        var updateRequest = new UpdateProductRequest
        {
            Name = "Ghost", Description = "N/A", Price = (decimal)1, Stock = 0
        };

        var response = await _client.PutAsJsonAsync($"/api/Product/{Guid.NewGuid()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task Delete_WithExistingProduct_ShouldReturn204()
    {
        // Create
        var request = new CreateProductRequest
        {
            Name = "Produit à supprimer",
            Description = "Temp",
            Price = (decimal)1,
            Stock = 1
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Product", request);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDTO>();

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/Product/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify it's gone
        var getResponse = await _client.GetAsync($"/api/Product/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithNonExistingProduct_ShouldReturn404()
    {
        var response = await _client.DeleteAsync($"/api/Product/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region PATCH (ChangePrice)

    [Fact]
    public async Task ChangePrice_WithValidIncrease_ShouldReturn200()
    {
        // Create
        var request = new CreateProductRequest
        {
            Name = "Casque Audio",
            Description = "Bluetooth",
            Price = (decimal)100,
            Stock = 20
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Product", request);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDTO>();

        // Change price (+30%, OK)
        var changePriceRequest = new ChangePriceRequest { Price = (decimal)130 };
        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/Product/{created!.Id}/price", changePriceRequest);

        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await patchResponse.Content.ReadFromJsonAsync<ProductDTO>();
        updated!.Price.Should().Be((decimal)130);
    }

    [Fact]
    public async Task ChangePrice_WithMoreThan50PercentIncrease_ShouldReturn400()
    {
        // Create
        var request = new CreateProductRequest
        {
            Name = "Webcam HD",
            Description = "1080p",
            Price = (decimal)100,
            Stock = 10
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Product", request);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDTO>();

        // Change price (+60%, NOT OK)
        var changePriceRequest = new ChangePriceRequest { Price = (decimal)160 };
        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/Product/{created!.Id}/price", changePriceRequest);

        patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}
