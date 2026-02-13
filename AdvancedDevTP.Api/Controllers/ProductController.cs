using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

/// <summary>
/// Contrôleur API pour la gestion des produits du catalogue.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur ProductController.
    /// </summary>
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Récupère tous les produits du catalogue.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// Récupère un produit spécifique par son identifiant.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDTO>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }


    /// <summary>
    /// Crée un nouveau produit dans le catalogue.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDTO>> Create([FromBody] CreateProductRequest request)
    {
        var product = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }


    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDTO>> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var product = await _productService.UpdateAsync(id, request);
        return Ok(product);
    }


    /// <summary>
    /// Supprime un produit du catalogue.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }


    /// <summary>
    /// Change le prix d'un produit.
    /// </summary>
    [HttpPatch("{id:guid}/price")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDTO>> ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
    {
        var product = await _productService.ChangePriceAsync(id, request);
        return Ok(product);
    }
}