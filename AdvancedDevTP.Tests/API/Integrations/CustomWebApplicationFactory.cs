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
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IProductRepositoryAsync));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IProductRepositoryAsync, InMemoryProductRepositoryAsync>();
        });

        builder.UseEnvironment("Testing");
    }
}