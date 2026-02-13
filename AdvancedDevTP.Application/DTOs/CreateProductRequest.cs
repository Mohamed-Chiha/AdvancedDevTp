using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Requête pour la création d'un nouveau produit.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Nom du produit à créer.
    /// </summary>
    [Required(ErrorMessage = "Entrez le nom")]
    [MaxLength(100, ErrorMessage = "Le non ne dépasse pas 100 charactères")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description du produit.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Prix unitaire du produit.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Prix doit etre positive")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantité initiale en stock.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être positif.")]
    public int Stock { get; set; }
}