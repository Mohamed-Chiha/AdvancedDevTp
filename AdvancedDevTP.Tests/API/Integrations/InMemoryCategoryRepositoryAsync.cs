using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

public class InMemoryCategoryRepositoryAsync : ICategoryRepositoryAsync
{
    private readonly List<Category> _categories = new();

    public Task<Category?> GetByIdAsync(Guid id)
        => Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Category>> GetAllAsync()
        => Task.FromResult<IEnumerable<Category>>(_categories.ToList());

    public Task AddAsync(Category category)
    {
        _categories.Add(category);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category)
    {
        var index = _categories.FindIndex(c => c.Id == category.Id);
        if (index >= 0) _categories[index] = category;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _categories.RemoveAll(c => c.Id == id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
        => Task.FromResult(_categories.Any(c => c.Id == id));
}