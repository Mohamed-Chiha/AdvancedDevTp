using AdvancedDevTP.Domain.Entities;

namespace AdvancedDevTP.Domain.Repositories;

/// <summary>
/// Interface de dépôt asynchrone pour la gestion des commandes.
/// </summary>
public interface IOrderRepositoryAsync
{
    /// <summary>
    /// Récupère une commande par son identifiant de manière asynchrone.
    /// </summary>
    Task<Order?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère toutes les commandes de manière asynchrone.
    /// </summary>
    Task<IEnumerable<Order>> GetAllAsync();
    
    /// <summary>
    /// Ajoute une nouvelle commande de manière asynchrone.
    /// </summary>
    Task AddAsync(Order order);
    
    /// <summary>
    /// Supprime une commande par son identifiant de manière asynchrone.
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Vérifie l'existence d'une commande par son identifiant.
    /// </summary>
    Task<bool> ExistsAsync(Guid id);
}