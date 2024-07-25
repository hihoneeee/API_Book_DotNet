using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestWebAPI.DTOs.User;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class userController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IRealTimeServices _realTimeServices;
        private readonly ISendMailServices _sendMailService;

        public userController(IUserServices userServices, IRealTimeServices realTimeServices, ISendMailServices sendMailService)
        {
            _userServices = userServices;
            _realTimeServices = realTimeServices;
            _sendMailService = sendMailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAsync()
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

            var serviceResponse = await _userServices.GetCurrentAsync(userId);

            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPut("change-profile")]
        public async Task<IActionResult> UpdateProfileUserAsync([FromBody] UserDTO userDTO)
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

            var serviceResponse = await _userServices.UpdateProfileUserAsync(userId, userDTO);

            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPut("change-avatar")]
        public async Task<IActionResult> UpdateAvatarUserAsync([FromForm] AvatarUserDTO avatarUserDTO)
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

            var serviceResponse = await _userServices.UpdateAvatarUserAsync(userId, avatarUserDTO);

            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [Authorize]
        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmailUserAysnc([FromBody] EmailUSerDTO emailUSerDTO)
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

            var serviceResponse = await _userServices.ChangeEmailUserAsync(userId, emailUSerDTO);
            if (serviceResponse.statusCode != EHttpType.Success)
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
            var emailBody = $"Please, click on the link below to change your email. This link will expire after 15 minutes: <a href='http://localhost:5173/confirm-change-email/{serviceResponse.accessToken}'>Change here!</a>";
            await _sendMailService.SendEmailAsync(emailUSerDTO.mewEmail, "Password Reset Request", emailBody);

            return Ok(new { serviceResponse.success, serviceResponse.message });


        }
        [HttpPost("confirm-change-email/{token}")]
        public async Task<IActionResult> ConfirmChangeEmailUserAsync(string token)
        {
            var serviceResponse = await _userServices.ConfirmChangeEmailUserAsync(token);

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
