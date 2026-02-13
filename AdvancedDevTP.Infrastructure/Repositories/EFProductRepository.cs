using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Data;
using AdvancedDevTP.Infrastructure.Entities;
using AdvancedDevTP.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevTP.Infrastructure.Repositories;

/// <summary>
/// Implémentation du dépôt de produits avec Entity Framework Core.
/// </summary>
public class EFProductRepository : IProductRepositoryAsync
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initialise une nouvelle instance du dépôt EFProductRepository.
    /// </summary>
    public EFProductRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Récupère un produit par son identifiant de manière asynchrone.
    /// </summary>
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    /// <summary>
    /// Récupère tous les produits de manière asynchrone.
    /// </summary>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var entities = await _context.Products.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain);
    }

    /// <summary>
    /// Ajoute un nouveau produit de manière asynchrone.
    /// </summary>
    public async Task AddAsync(Product product)
    {
        var entity = MapToEntity(product);
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Met à jour un produit existant de manière asynchrone.
    /// </summary>
    public async Task UpdateAsync(Product product)
    {
        var entity = await _context.Products.FindAsync(product.Id);
        if (entity is null)
            throw new InfrastructureException($"Produit avec l'id '{product.Id}' introuvable en base.");

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Price = product.Price;
        entity.Stock = product.Stock;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Supprime un produit par son identifiant de manière asynchrone.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity is null)
            throw new InfrastructureException($"Produit avec l'id '{id}' introuvable en base.");

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Vérifie l'existence d'un produit par son identifiant.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    /// <summary>
    /// Change le prix d'un produit par son identifiant.
    /// </summary>
    public Task ChangePriceAsync(Guid id, decimal newPrice)
    {
        throw new NotImplementedException();
    }

    #region Mapping

    /// <summary>
    /// Mappe une entité ProductEntity vers un objet de domaine Product.
    /// </summary>
    private static Product MapToDomain(ProductEntity entity)
    {
        var product = (Product)System.Runtime.Serialization.FormatterServices
            .GetUninitializedObject(typeof(Product));

        var type = typeof(Product);
        type.GetProperty(nameof(Product.Id))!.SetValue(product, entity.Id);
        type.GetProperty(nameof(Product.Name))!.SetValue(product, entity.Name);
        type.GetProperty(nameof(Product.Description))!.SetValue(product, entity.Description);
        type.GetProperty(nameof(Product.Price))!.SetValue(product, entity.Price);
        type.GetProperty(nameof(Product.Stock))!.SetValue(product, entity.Stock);
        type.GetProperty(nameof(Product.IsActive))!.SetValue(product, entity.IsActive);

        return product;
    }

    /// <summary>
    /// Mappe un objet de domaine Product vers une entité ProductEntity.
    /// </summary>
    private static ProductEntity MapToEntity(Product product)
    {
        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive =  product.IsActive,
        };
    }

    #endregion
}
