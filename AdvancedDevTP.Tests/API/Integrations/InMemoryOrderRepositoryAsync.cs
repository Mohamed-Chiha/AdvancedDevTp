using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Repositories;

namespace AdvancedDevTP.Tests.API.Integrations;

/// <summary>
/// Implémentation en mémoire du dépôt de commandes pour les tests d'intégration.
/// Stocke les commandes dans une liste en mémoire au lieu d'une base de données.
/// </summary>
public class InMemoryOrderRepositoryAsync : IOrderRepositoryAsync
{
    private readonly List<Order> _orders = new();

    /// <summary>
    /// Récupère une commande par son identifiant.
    /// </summary>
    public Task<Order?> GetByIdAsync(Guid id)
        => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    /// <summary>
    /// Récupère toutes les commandes.
    /// </summary>
    public Task<IEnumerable<Order>> GetAllAsync()
        => Task.FromResult<IEnumerable<Order>>(_orders.ToList());

    /// <summary>
    /// Ajoute une nouvelle commande à la collection en mémoire.
    /// </summary>
    public Task AddAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Supprime une commande par son identifiant.
    /// </summary>
    public Task DeleteAsync(Guid id)
    {
        _orders.RemoveAll(o => o.Id == id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Vérifie l'existence d'une commande par son identifiant.
    /// </summary>
    public Task<bool> ExistsAsync(Guid id)
        => Task.FromResult(_orders.Any(o => o.Id == id));
}