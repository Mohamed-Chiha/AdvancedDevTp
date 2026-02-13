using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

/// <summary>
/// Contrôleur API pour la gestion des commandes clients.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur OrderController.
    /// </summary>
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Récupère toutes les commandes.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        => Ok(await _orderService.GetAllAsync());

    /// <summary>
    /// Récupère une commande spécifique par son identifiant.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDTO>> GetById(Guid id)
        => Ok(await _orderService.GetByIdAsync(id));

    /// <summary>
    /// Crée une nouvelle commande.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> Create([FromBody] CreateOrderRequest request)
    {
        var order = await _orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Supprime une commande.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}