using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.Helpers;
using TestWebAPI.Services.Interfaces;

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
            try
            {
                var serviceResponse = await _authService.Register(authRegisterDTO);
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getMessage());
                }
                else
                {
                    return BadRequest(serviceResponse);
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthLoginDTO authLoginDTO)
        {
            try
            {
                var serviceResponse = await _authService.Login(authLoginDTO);
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getData());
                }
                else
                {
                    return BadRequest(serviceResponse);
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }
    }
}
