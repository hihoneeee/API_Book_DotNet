using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificationController : ControllerBase
    {
        private readonly IRealTimeServices _realTimeServices;

        public notificationController(IRealTimeServices realTimeServices)
        {
            _realTimeServices = realTimeServices;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotificationsForUser(int userId)
        {
            var serviceResponse = await _realTimeServices.GetNotificationsForUser(userId);
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
            var serviceResponse = await _realTimeServices.MarkNotificationsAsRead(userId);
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
