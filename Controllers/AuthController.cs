using Azure;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISendMailService _sendMailService;

        public AuthController(IAuthService authService, ISendMailService sendMailService)
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
                    return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.access_token });
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
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.access_token });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] AuthResetPasswordDTO authChangePasswordDTO)
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
        public async Task<IActionResult> ResetPasswordAsync([FromBody] AuthResetPassword authResetPassword, [FromQuery] string token )
        {
            var serviceResponse = await _authService.ResetPasswordAsync(authResetPassword.password, token);
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
