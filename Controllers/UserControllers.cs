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
    public class userController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IRealTimeServices _realTimeServices;

        public userController(IUserServices userServices, IRealTimeServices realTimeServices)
        {
            _userServices = userServices;
            _realTimeServices = realTimeServices;

        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAsync(int id)
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
        [HttpPost("connected")]
        public async Task<IActionResult> OnConnectedAsync(int id, string connectionId)
        {
            await _realTimeServices.OnConnectedAsync(id, connectionId);
            return Ok();
        }

        [HttpPost("disconnected")]
        public async Task<IActionResult> OnDisconnectedAsync(int id, string connectionId)
        {
            await _realTimeServices.OnDisconnectedAsync(id, connectionId);
            return Ok();
        }

    }
}
