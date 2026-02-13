using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

/// <summary>
/// Entité de domaine représentant une catégorie de produits.
/// </summary>
public class Category
{
    /// <summary>
    /// Identifiant unique de la catégorie.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Nom de la catégorie.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Description de la catégorie.
    /// </summary>
    public string Description { get; private set; } = string.Empty;
    private Category() { }

    /// <summary>
    /// Crée une nouvelle instance de catégorie avec validation des données.
    /// </summary>
    public Category(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom de la catégorie est obligatoire.");

        if (name.Length > 100)
            throw new DomainException("Le nom de la catégorie ne peut pas dépasser 100 caractères.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description ?? string.Empty;
    }

    /// <summary>
    /// Met à jour les informations de la catégorie avec validation.
    /// </summary>
    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom de la catégorie est obligatoire.");

        Name = name;
        Description = description ?? string.Empty;
    }
}