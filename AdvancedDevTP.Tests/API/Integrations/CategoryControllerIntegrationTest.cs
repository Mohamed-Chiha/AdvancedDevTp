using System.Net;
using System.Net.Http.Json;
using AdvancedDevTP.Application.DTOs;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.Integrations;

public class CategoryControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CategoryControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturn201()
    {
        var request = new CreateCategoryRequest
        {
            Name = "Électronique",
            Description = "Appareils électroniques"
        };

        var response = await _client.PostAsJsonAsync("/api/Category", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<CategoryDTO>();
        created!.Name.Should().Be("Électronique");
    }

    [Fact]
    public async Task Create_ThenGetById_ShouldReturnSameCategory()
    {
        var request = new CreateCategoryRequest { Name = "Vêtements", Description = "Mode" };
        var createResponse = await _client.PostAsJsonAsync("/api/Category", request);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryDTO>();

        var getResponse = await _client.GetAsync($"/api/Category/{created!.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrieved = await getResponse.Content.ReadFromJsonAsync<CategoryDTO>();
        retrieved!.Name.Should().Be("Vêtements");
    }

    [Fact]
    public async Task Update_WithExistingCategory_ShouldReturn200()
    {
        var createRequest = new CreateCategoryRequest { Name = "Old", Description = "Old desc" };
        var createResponse = await _client.PostAsJsonAsync("/api/Category", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryDTO>();

        var updateRequest = new UpdateCategoryRequest { Name = "New", Description = "New desc" };
        var updateResponse = await _client.PutAsJsonAsync($"/api/Category/{created!.Id}", updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await updateResponse.Content.ReadFromJsonAsync<CategoryDTO>();
        updated!.Name.Should().Be("New");
    }

    [Fact]
    public async Task Delete_WithExistingCategory_ShouldReturn204()
    {
        var request = new CreateCategoryRequest { Name = "À supprimer", Description = "Temp" };
        var createResponse = await _client.PostAsJsonAsync("/api/Category", request);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryDTO>();

        var deleteResponse = await _client.DeleteAsync($"/api/Category/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetById_WithNonExisting_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/Category/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}