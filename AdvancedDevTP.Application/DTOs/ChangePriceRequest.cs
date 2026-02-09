using System.ComponentModel.DataAnnotations;

namespace AdvancedDevTP.Application.DTOs;

public class ChangePriceRequest
{
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}