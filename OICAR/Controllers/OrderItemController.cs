using Microsoft.AspNetCore.Mvc;
using OICAR.Service.Interface;
using OICAR.Models;
using OICAR.DTOs;

namespace OICAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAll()
        {
            var orderItems = await _orderItemService.GetAllAsync();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDTO>> GetById(int id)
        {
            var orderItem = await _orderItemService.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> Add(OrderItemDTO orderItem)
        {
            await _orderItemService.AddAsync(orderItem);
            return CreatedAtAction(nameof(GetById), new { id = orderItem.IDOrderItem }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItemDTO orderItem)
        {
            if (id != orderItem.IDOrderItem)
            {
                return BadRequest();
            }

            await _orderItemService.UpdateAsync(orderItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderItemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("orders/{orderId}/items")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetByOrderId(int orderId)
        {
            var orderItems = await _orderItemService.GetByOrderIdAsync(orderId);
            return Ok(orderItems);
        }

        [HttpGet("items/{itemId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetByItemId(int itemId)
        {
            var orderItems = await _orderItemService.GetByItemIdAsync(itemId);
            return Ok(orderItems);
        }
    }
}
