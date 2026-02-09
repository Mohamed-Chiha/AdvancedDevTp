using AdvancedDevTP.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AdvancedDevTP.Tests.API.Integrations;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Supprimer le vrai repository enregistré dans Program.cs
            services.RemoveAll(typeof(IProductRepositoryAsync));

            // Ajouter la version In-Memory pour les tests
            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();
        });
    }
}