﻿namespace AdvancedDevTP.Infrastructure.Entities;

/// <summary>
/// Entité de persistence pour une catégorie en base de données.
/// </summary>
public class CategoryEntity
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