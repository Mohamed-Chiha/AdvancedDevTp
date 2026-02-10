using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Data;
using AdvancedDevTP.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevTP.Infrastructure.Repositories;

public class EFCategoryRepository : ICategoryRepositoryAsync
{
    private readonly AppDbContext _context;

    public EFCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Categories.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var entities = await _context.Categories.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(Category category)
    {
        var entity = new CategoryEntity
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        var entity = await _context.Categories.FindAsync(category.Id);
        if (entity is null) return;
        entity.Name = category.Name;
        entity.Description = category.Description;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity is null) return;
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    private static Category MapToDomain(CategoryEntity entity)
    {
        var category = (Category)System.Runtime.Serialization.FormatterServices
            .GetUninitializedObject(typeof(Category));
        var type = typeof(Category);
        type.GetProperty(nameof(Category.Id))!.SetValue(category, entity.Id);
        type.GetProperty(nameof(Category.Name))!.SetValue(category, entity.Name);
        type.GetProperty(nameof(Category.Description))!.SetValue(category, entity.Description);
        return category;
    }
}