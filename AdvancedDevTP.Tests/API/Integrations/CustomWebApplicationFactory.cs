using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AdvancedDevTP.Tests.API.Integrations;

/// <summary>
/// Factory personnalisée pour créer une application web en mémoire pour les tests d'intégration.
/// Utilise des dépôts en mémoire au lieu des vrais dépôts Entity Framework.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// Configure le host de test en remplaçant les dépôts par des implémentations en mémoire.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // 1. SUPPRESSION RADICALE
            // On retire toute trace de IProductRepositoryAsync (la version EF Core du Program.cs)
            services.RemoveAll<IProductRepositoryAsync>();
            
            // On retire aussi le DbContext pour être sûr qu'il n'interfère pas
            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();

            // 2. (Optionnel) Si on a besoin du DbContext pour d'autres trucs, on remet une version InMemory factice
            // Mais pour ce test, c'est le Repository qui compte.
            services.AddDbContext<AppDbContext>(options => 
                options.UseInMemoryDatabase("TestDb_Ignore"));

            // 3. INJECTION DU MOCK EN SINGLETON
            // C'est lui qui contient la List<Product> persistante
            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();
            
            // Faire de même pour les autres si nécessaire
            services.RemoveAll<ICategoryRepositoryAsync>();
            services.AddSingleton<ICategoryRepositoryAsync, InMemoryCategoryRepositoryAsync>();
            
            services.RemoveAll<IOrderRepositoryAsync>();
            services.AddSingleton<IOrderRepositoryAsync, InMemoryOrderRepositoryAsync>();
        });
    }
}