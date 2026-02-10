using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        => Ok(await _categoryService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        => Ok(await _categoryService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Create([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDTO>> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        => Ok(await _categoryService.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}