using E_Commerce.Application.DTOs.AuthDTOs;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
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
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterCustomerDto register)
        {
            var res = await _authService.RegisterAsync(register);
            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var res = await _authService.LoginAsync(loginDto);
            return Ok(res);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var res = await _authService.RefreshTokenAsync(refreshTokenDto);
            return Ok(res);
        }
    }
}
