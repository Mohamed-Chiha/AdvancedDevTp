using AdvancedDevTP.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AdvancedDevTP.Tests.API.Integrations;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing repositories
            RemoveService<IProductRepositoryAsync>(services);
            RemoveService<ICategoryRepositoryAsync>(services);
            RemoveService<IOrderRepositoryAsync>(services);

            // Register in-memory repositories
            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();
            services.AddSingleton<ICategoryRepositoryAsync, InMemoryCategoryRepositoryAsync>();
            services.AddSingleton<IOrderRepositoryAsync, InMemoryOrderRepositoryAsync>();
        });

        builder.UseEnvironment("Testing");
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null) services.Remove(descriptor);
    }
}