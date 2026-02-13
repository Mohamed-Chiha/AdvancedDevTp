using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Requête pour la mise à jour d'un produit existant.
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Nouveau nom du produit.
    /// </summary>
    [Required(ErrorMessage = "nom obligatoire.")]
    [MaxLength(100, ErrorMessage = "Nom doit etre > 100")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nouvelle description du produit.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Nouveau prix unitaire du produit.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif.")]
    public decimal Price { get; set; }

    /// <summary>
    /// Nouvelle quantité en stock.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être positif.")]
    public int Stock { get; set; }
}