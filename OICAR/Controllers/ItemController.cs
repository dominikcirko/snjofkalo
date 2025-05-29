using Microsoft.AspNetCore.Mvc;
using OICAR.DTOs;
using OICAR.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OICAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAll()
        {
            var allItems = await _itemService.GetAllAsync();
            return Ok(allItems);
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult<ItemDTO>> GetById(int itemId)
        {
            var item = await _itemService.GetByIdAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> Add(ItemDTO item)
        {
            await _itemService.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { itemId = item.IDItem }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ItemDTO item)
        {
            if (id != item.IDItem)
            {
                return BadRequest();
            }

            await _itemService.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("categories/{categoryId}/items")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetByCategoryId(int categoryId)
        {
            var items = await _itemService.GetByCategoryIdAsync(categoryId);
            return Ok(items);
        }

        //[HttpGet("tags/{tagId?}/items")]
        //public async Task<ActionResult<IEnumerable<ItemDTO>>> GetByTagId(int? tagId)
        //{
        //    var items = await _itemService.GetByTagIdAsync(tagId);
        //    return Ok(items);
        //}

        [HttpGet("items/title/search")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> SearchByTitle([FromQuery] string title)
        {
            var items = await _itemService.SearchByTitleAsync(title);
            return Ok(items);
        }

        [HttpGet("items/{itemId}/stock")]
        public async Task<ActionResult<bool>> IsInStock(int itemId)
        {
            var inStock = await _itemService.IsInStockAsync(itemId);
            return Ok(inStock);
        }
    }
}
