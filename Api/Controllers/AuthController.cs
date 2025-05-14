using System.Threading.Tasks;
using Application.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;


namespace Notification.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, token, userId) = await _authService.RegisterAsync(registerDto.Email, registerDto.Password);

            if (!success)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Registration failed. Email already exists."
                });
            }

            return Ok(new AuthResponseDto
            {
                Success = true,
                Token = token,
                UserId = userId,
                Message = "Registration successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, token, userId) = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

            if (!success)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            return Ok(new AuthResponseDto
            {
                Success = true,
                Token = token,
                UserId = userId,
                Message = "Login successful"
            });
        }
    }
}