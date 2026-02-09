using AdvancedDevTP.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevTP.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });
    }
}