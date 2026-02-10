
using AdvancedDevSample.API.Middlewares;
using AdvancedDevTP.Application.Interfaces;
using AdvancedDevTP.Domain.Repositories;
using AdvancedDevTP.Infrastructure.Data;
using AdvancedDevTP.Infrastructure.Repositories;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductCatalogDb"));

// --- Injection de dépendances ---
builder.Services.AddScoped<IProductRepositoryAsync, EFProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepositoryAsync, EFCategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IOrderRepositoryAsync, EFOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// --- Controllers ---
builder.Services.AddControllers();

// --- Swagger / OpenAPI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AdvancedDevTP - Catalogue Produit API",
        Version = "v1",
        Description = "API REST pour la gestion d'un catalogue de produits. Architecture en couches (Domain, Application, Infrastructure, API).",
        Contact = new OpenApiContact
        {
            Name = "Mohamed Chiha"
        }
    });

    // Inclure les commentaires XML pour la documentation Swagger
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (var xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile);
    }
});

var app = builder.Build();

// --- Middleware de gestion globale des exceptions ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

// --- Swagger (disponible en dev et en prod pour la démo) ---
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalogue Produit API v1");
    options.RoutePrefix = string.Empty; // Swagger accessible à la racine
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Nécessaire pour les tests d'intégration avec WebApplicationFactory
public partial class Program { }
