using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public userController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAsync(int id)
        {
            var userIdClaim = HttpContext.User.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { success = false, message = "Invalid token!" });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { success = false, message = "Invalid user in token!" });
            }
            var serviceResponse = await _userServices.GetCurrentAsync((int.Parse(userIdClaim)));

            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {    
                 return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
    }
}
