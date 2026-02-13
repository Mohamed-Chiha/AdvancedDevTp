using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Requête pour la création d'une nouvelle catégorie.
/// </summary>
public class CreateCategoryRequest
{
    /// <summary>
    /// Nom de la catégorie à créer.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description de la catégorie.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Requête pour la mise à jour d'une catégorie existante.
/// </summary>
public class UpdateCategoryRequest
{
    /// <summary>
    /// Nouveau nom de la catégorie.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Nouvelle description de la catégorie.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Objet de transfert de données représentant une catégorie de produits.
/// </summary>
public class CategoryDTO
{
    /// <summary>
    /// Identifiant unique de la catégorie.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom de la catégorie.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description de la catégorie.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}