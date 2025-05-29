using Microsoft.AspNetCore.Mvc;
using OICAR.DTOs;
using OICAR.Service.Interface;

namespace webshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetById(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<TagDTO>> Add(TagDTO tag)
        {
            await _tagService.AddAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id = tag.IDTag }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TagDTO tag)
        {
            if (id != tag.IDTag)
            {
                return BadRequest();
            }

            await _tagService.UpdateAsync(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<TagDTO>> GetByName(string name)
        {
            var tag = await _tagService.GetByNameAsync(name);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }
    }
}
