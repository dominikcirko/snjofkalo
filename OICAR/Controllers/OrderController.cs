using Microsoft.AspNetCore.Mvc;
using OICAR.Service.Interface;
using OICAR.Models;
using OICAR.DTOs;

namespace OICAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Add(OrderDTO order)
        {
            await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.IDOrder }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDTO order)
        {
            if (id != order.IDOrder)
            {
                return BadRequest();
            }

            await _orderService.UpdateAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByUserId(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] int statusId)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, statusId);
            return NoContent();
        }
    }
}
