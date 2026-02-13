using AdvancedDevTP.Application.DTOs;

namespace AdvancedDevTP.Application.Interfaces;

/// <summary>
/// Service métier pour la gestion des commandes clients.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Récupère une commande par son identifiant.
    /// </summary>
    Task<OrderDTO> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Récupère toutes les commandes.
    /// </summary>
    Task<IEnumerable<OrderDTO>> GetAllAsync();
    
    /// <summary>
    /// Crée une nouvelle commande.
    /// </summary>
    Task<OrderDTO> CreateAsync(CreateOrderRequest request);
    
    /// <summary>
    /// Supprime une commande.
    /// </summary>
    Task DeleteAsync(Guid id);
}