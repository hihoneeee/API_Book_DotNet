using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISendMailServices _sendMailService;

        public authController(IAuthService authService, ISendMailServices sendMailService)
        {
            _authService = authService;
            _sendMailService = sendMailService;
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
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.accessToken, serviceResponse.refreshToken });
                }
                else
                {
                    return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
                }
        }

        [Route("refresh-token")]
        [HttpPost]
        public async Task<IActionResult> refreshTokenAsync ([FromBody] RefreshTokenDTO refreshToken)
        {
            var serviceResponse = await _authService.refreshTokenAsync(refreshToken.token);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.accessToken });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] AuthForgotPasswordDTO authChangePasswordDTO)
        {
            var serviceResponse = await _authService.ForgotPassword(authChangePasswordDTO.email);

            if (serviceResponse.statusCode != EHttpType.Success)
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

            var emailBody = $"Please, click on the link below to change your password. This link will expire after 15 minutes: <a href='https://localhost:7107/api/auth/reset-password/{serviceResponse.data.passwordResetToken}'>Change here!</a>";

            await _sendMailService.SendEmailAsync(authChangePasswordDTO.email, "Password Reset Request", emailBody);

            return Ok(new { serviceResponse.success, serviceResponse.message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] AuthResetPasswordDTO authChangePasswordDTO, [FromQuery] string token )
        {
            var serviceResponse = await _authService.ResetPasswordAsync(authChangePasswordDTO.password, token);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordasync([FromBody] AuthChangePasswordDTO authChangePasswordDTO)
        {
            var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { success = false, message = "Invalid token!" });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { success = false, message = "Invalid user in token!" });
            }
            authChangePasswordDTO.id = userId;
            var serviceResponse = await _authService.ChangePasswordasync(authChangePasswordDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
    }
}
