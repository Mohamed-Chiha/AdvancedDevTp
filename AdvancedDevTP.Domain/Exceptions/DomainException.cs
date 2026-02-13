namespace AdvancedDevTP.Domain.Exceptions;

/// <summary>
/// Exception levée lors de violations des règles métier du domaine.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Crée une nouvelle instance d'exception de domaine.
    /// </summary>
    public DomainException(string message) : base(message)
    {
    }
}