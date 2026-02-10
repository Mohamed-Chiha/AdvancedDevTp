using AdvancedDevTP.Application.DTOs;
using AdvancedDevTP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevTP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        => Ok(await _orderService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDTO>> GetById(Guid id)
        => Ok(await _orderService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> Create([FromBody] CreateOrderRequest request)
    {
        var order = await _orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}