using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Exceptions;
using AdvancedDevTP.Application.Interfaces;
using AdvancedDevTP.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPut("{id}/price")]
    public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
    {
        try
        {
            _productService.ChangeProductPrice(id, request.Price);
            return NoContent(); //204
        }
        catch (ApplicationServiceException ex)
        {
            return NotFound(ex.Message); //404
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message); //400
        }
    }
}