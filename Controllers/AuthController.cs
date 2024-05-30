using Azure;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.Helpers;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO authRegisterDTO)
        {
                var serviceResponse = await _authService.Register(authRegisterDTO);
                if (serviceResponse.statusCode == EHttpType.Success)
                {
                    return Ok(new { serviceResponse.success, serviceResponse.message });
                }
                else
                {
                    return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
                }
        }
        [Route("login")]
        [HttpPost]
        
        public async Task<IActionResult> Login([FromBody] AuthLoginDTO authLoginDTO)
        {
                var serviceResponse = await _authService.Login(authLoginDTO);
                if (serviceResponse.statusCode == EHttpType.Success)
                {
                    return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.access_token });
                }
                else
                {
                    return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
                }
        }

        [Route("refresh-token")]
        [HttpPost]
        public async Task<IActionResult> refreshTokenAsync ([FromBody] refreshToken refreshToken)
        {
            var serviceResponse = await _authService.refreshTokenAsync(refreshToken.token);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.access_token });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

        }
    }
}
