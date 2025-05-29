using Microsoft.AspNetCore.Mvc;
using OICAR.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using OICAR.DTOs;

namespace OICAR.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetAll()
        {
            var cartItems = await _cartItemService.GetAllAsync();
            return Ok(cartItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> GetById(int id)
        {
            var cartItem = await _cartItemService.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> Add([FromBody] CartItemDTO cartItem)
        {
            if (cartItem == null || cartItem.ItemID <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest("Invalid item or quantity.");
            }

            await _cartItemService.AddAsync(cartItem);
            return CreatedAtAction(nameof(GetById), new { id = cartItem.IDCartItem }, cartItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CartItemDTO cartItem)
        {
            if (id != cartItem.IDCartItem)
            {
                return BadRequest();
            }

            await _cartItemService.UpdateAsync(cartItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartItemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetByCartId(int cartId)
        {
            var cartItems = await _cartItemService.GetByCartIdAsync(cartId);
            return Ok(cartItems);
        }
    }
}
