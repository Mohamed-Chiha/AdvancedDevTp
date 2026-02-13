using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using AdvancedDevTP.Infrastructure.Exceptions;

namespace AdvancedDevSample.API.Middlewares
{
    /// <summary>
    /// Middleware pour la gestion centralisée des exceptions en couche API.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        
        /// <summary>
        /// Initialise une nouvelle instance du middleware ExceptionHandlingMiddleware.
        /// </summary>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    /// <summary>
    /// Traite les exceptions levées durant le pipeline de requête.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException domainEx)
        {
            _logger.LogError(domainEx, "Erreur metier");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = "Erreur metier", detail = domainEx.Message });
        }
        catch (ApplicationServiceException ex)
        {
            _logger.LogWarning(ex, "Erreur Applicative");
            
            context.Response.StatusCode = (int)ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new { error = "Ressource Introuvable", detail = ex.Message });
        }
        catch (InfrastructureException ex)
        {
            _logger.LogWarning(ex, "Erreur Infrastructure");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Erreur Infrastructure", detail = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Erreur innatendue");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Erreur Interne" }) );
        }
    }
}
}