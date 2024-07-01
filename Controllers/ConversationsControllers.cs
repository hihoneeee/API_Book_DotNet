using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class conversationsController : ControllerBase
    {
        private readonly IChatHubServices _chatHubServices;

        public conversationsController(IChatHubServices chatHubServices)
        {
            _chatHubServices = chatHubServices;
        }

        [HttpPost("GetOrCreate")]
        public async Task<IActionResult> GetOrCreateConversation([FromBody] ConversationDTO conversationDTO)
        {
            var serviceResponse = await _chatHubServices.GetOrCreateConversation(conversationDTO);
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
            await _chatHubServices.JoinConversation(conversationId, connectionId);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveConversation(int conversationId, string connectionId)
        {
            await _chatHubServices.LeaveConversation(conversationId, connectionId);
            return Ok();
        }
    }
}
