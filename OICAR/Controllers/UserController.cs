using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using OICAR.Service.Interface;
using OICAR.DTOs;

namespace OICAR.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var authenticatedUserId = int.Parse(User.FindFirst("id").Value);

            if (id != authenticatedUserId)
            {
                return Forbid();
            }

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Add(UserDTO user)
        {
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.IDUser }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDTO user)
        {
            var authenticatedUserId = int.Parse(User.FindFirst("id").Value);

            if (id != authenticatedUserId)
            {
                return Forbid();
            }

            if (id != user.IDUser)
            {
                return BadRequest();
            }

            await _userService.UpdateAsync(user);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<UserDTO>> GetByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult<UserDTO>> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAdmins()
        {
            var admins = await _userService.GetAdminsAsync();
            return Ok(admins);
        }
    }
}