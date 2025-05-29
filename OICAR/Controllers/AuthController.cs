using Microsoft.AspNetCore.Mvc;
using OICAR.DTOs;
using OICAR.Service.Interface;
using System.Security.Cryptography;
using System.Text;

namespace OICAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;


        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _userService.GetByEmailAsync(registerDTO.Email);
            if (existingUser != null)
                return BadRequest("User already exists");

            using var hmac = new HMACSHA256();
            var passwordSalt = hmac.Key;
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)));

            var user = new UserDTO
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = registerDTO.PhoneNumber,
                IsAdmin = false
            };

            await _userService.AddAsync(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (Request.Cookies.ContainsKey("JwtToken"))
                Response.Cookies.Delete("JwtToken");


            var user = await _userService.GetByEmailAsync(loginDTO.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            using var hmac = new HMACSHA256(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            if (Convert.ToBase64String(computedHash) != user.Password)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}