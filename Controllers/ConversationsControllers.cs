using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class conversationsController : ControllerBase
    {
        private readonly IRealTimeServices _realTimeServices;
        private readonly IConversationServices _conversationServices;

        public conversationsController(IRealTimeServices realTimeServices, IConversationServices conversationServices)
        {
            _realTimeServices = realTimeServices;
            _conversationServices = conversationServices;
        }

        [HttpPost("GetOrCreate")]
        public async Task<IActionResult> GetOrCreateConversation([FromBody] ConversationDTO conversationDTO)
        {
            var serviceResponse = await _realTimeServices.GetOrCreateConversation(conversationDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetConversationByUserId()
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

            var serviceResponse = await _conversationServices.GetConversationByUserId(userId);
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
