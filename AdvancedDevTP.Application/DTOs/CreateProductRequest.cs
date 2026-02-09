using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Entrez le nom")]
    [MaxLength(100, ErrorMessage = "Le non ne dépasse pas 100 charactères")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Prix doit etre positive")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être positif.")]
    public int Stock { get; set; }
}