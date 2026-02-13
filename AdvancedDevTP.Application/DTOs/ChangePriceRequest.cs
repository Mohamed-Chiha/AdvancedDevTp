using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

/// <summary>
/// Requête pour la modification du prix d'un produit.
/// </summary>
public class ChangePriceRequest
{
    /// <summary>
    /// Nouveau prix du produit.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}