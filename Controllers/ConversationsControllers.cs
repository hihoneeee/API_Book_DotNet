using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class conversationsController : ControllerBase
    {
        private readonly IRealTimeServices _realTimeServices;

        public conversationsController(IRealTimeServices realTimeServices)
        {
            _realTimeServices = realTimeServices;
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

        [HttpPost("join")]
        public async Task<IActionResult> JoinConversation(int conversationId, string connectionId)
        {
            await _realTimeServices.JoinConversation(conversationId, connectionId);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveConversation(int conversationId, string connectionId)
        {
            await _realTimeServices.LeaveConversation(conversationId, connectionId);
            return Ok();
        }
    }
}
