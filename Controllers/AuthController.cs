using Microsoft.AspNetCore.Mvc;
using dotnet_webapi_claude_wrapper.Contracts;
using dotnet_webapi_claude_wrapper.Services;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_webapi_claude_wrapper.Controllers
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

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _authService.Login(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            try
            {
                var response = await _authService.Register(registerDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> ChangePassword(string newPassword)
        {
            var response = await _authService.ChangePassword(newPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}