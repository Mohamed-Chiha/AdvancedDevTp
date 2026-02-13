using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

/// <summary>
/// Contrôleur API pour la gestion des catégories de produits.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur CategoryController.
    /// </summary>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Récupère toutes les catégories.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        => Ok(await _categoryService.GetAllAsync());

    /// <summary>
    /// Récupère une catégorie spécifique par son identifiant.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        => Ok(await _categoryService.GetByIdAsync(id));

    /// <summary>
    /// Crée une nouvelle catégorie.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Create([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    /// <summary>
    /// Met à jour une catégorie existante.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDTO>> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        => Ok(await _categoryService.UpdateAsync(id, request));

    /// <summary>
    /// Supprime une catégorie.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}