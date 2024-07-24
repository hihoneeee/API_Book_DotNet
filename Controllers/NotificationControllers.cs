using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificationController : ControllerBase
    {
        private readonly IRealTimeServices _realTimeServices;
        private readonly INotificationServices _notificationServices;

        public notificationController(IRealTimeServices realTimeServices, INotificationServices notificationServices)
        {
            _realTimeServices = realTimeServices;
            _notificationServices = notificationServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsForUser()
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
            var serviceResponse = await _notificationServices.GetNotificationsForUser(userId);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPost("markAsRead/{userId}")]
        public async Task<IActionResult> MarkNotificationsAsRead(int userId)
        {
            var serviceResponse = await _notificationServices.MarkNotificationsAsRead(userId);
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
