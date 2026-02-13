using System.Net;

namespace AdvancedDevTP.Application.Exceptions;

/// <summary>
/// Exception levée par les services applicatifs lors d'erreurs métier.
/// </summary>
public class ApplicationServiceException : Exception 
{ 
    /// <summary>
    /// Code de statut HTTP associé à l'exception.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    
    /// <summary>
    /// Crée une nouvelle instance d'exception de service applicatif.
    /// </summary>
    public ApplicationServiceException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base (message) 
    {
        StatusCode = statusCode;
    }
}