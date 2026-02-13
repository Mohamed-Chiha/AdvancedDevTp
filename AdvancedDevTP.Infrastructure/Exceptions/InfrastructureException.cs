namespace AdvancedDevTP.Infrastructure.Exceptions;

/// <summary>
/// Exception levée lors d'erreurs de la couche d'infrastructure.
/// </summary>
public class InfrastructureException : Exception
{
    /// <summary>
    /// Crée une nouvelle instance d'exception d'infrastructure sans message.
    /// </summary>
    public InfrastructureException()
    {
    }
    
    /// <summary>
    /// Crée une nouvelle instance d'exception d'infrastructure avec un message.
    /// </summary>
    public InfrastructureException(string message)
        : base(message)
    {
    }
    
    /// <summary>
    /// Crée une nouvelle instance d'exception d'infrastructure avec un message et une exception interne.
    /// </summary>
    public InfrastructureException(string message, Exception innerException) : base(message, innerException) { }

}