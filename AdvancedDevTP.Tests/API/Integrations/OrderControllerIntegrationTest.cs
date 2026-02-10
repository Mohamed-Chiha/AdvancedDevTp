using System.Net;
using System.Net.Http.Json;
using AdvancedDevTP.Application.DTOs;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.Integrations;

public class OrderControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OrderControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<ProductDTO> CreateTestProduct()
    {
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "For order testing",
            Stock = 50,
            Price = 99.99m
        };
        var response = await _client.PostAsJsonAsync("/api/Product", request);
        return (await response.Content.ReadFromJsonAsync<ProductDTO>())!;
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturn201()
    {
        var product = await CreateTestProduct();

        var request = new CreateOrderRequest
        {
            CustomerName = "Mohamed",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = product.Id, Quantity = 2 }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/Order", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<OrderDTO>();
        created!.CustomerName.Should().Be("Mohamed");
        created.Items.Should().HaveCount(1);
        created.TotalAmount.Should().Be(199.98m);
    }

    [Fact]
    public async Task Create_ThenGetById_ShouldReturnSameOrder()
    {
        var product = await CreateTestProduct();

        var request = new CreateOrderRequest
        {
            CustomerName = "Ali",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = product.Id, Quantity = 1 }
            }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Order", request);
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDTO>();

        var getResponse = await _client.GetAsync($"/api/Order/{created!.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrieved = await getResponse.Content.ReadFromJsonAsync<OrderDTO>();
        retrieved!.CustomerName.Should().Be("Ali");
    }

    [Fact]
    public async Task Create_WithNonExistingProduct_ShouldReturn404()
    {
        var request = new CreateOrderRequest
        {
            CustomerName = "Test",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/Order", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithExistingOrder_ShouldReturn204()
    {
        var product = await CreateTestProduct();
        var request = new CreateOrderRequest
        {
            CustomerName = "À supprimer",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = product.Id, Quantity = 1 }
            }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/Order", request);
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDTO>();

        var deleteResponse = await _client.DeleteAsync($"/api/Order/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WithNonExistingOrder_ShouldReturn404()
    {
        var response = await _client.DeleteAsync($"/api/Order/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}