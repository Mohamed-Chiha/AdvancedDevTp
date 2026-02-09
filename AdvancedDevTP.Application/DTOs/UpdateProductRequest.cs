using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

public class UpdateProductRequest
{
    [Required(ErrorMessage = "nom obligatoire.")]
    [MaxLength(100, ErrorMessage = "Nom doit etre > 100")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être positif.")]
    public int Stock { get; set; }
}