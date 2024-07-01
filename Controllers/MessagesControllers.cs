using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class messagesController : ControllerBase
    {
        private readonly IChatHubServices _chatHubServices;

        public messagesController(IChatHubServices chatHubServices)
        {
            _chatHubServices = chatHubServices;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {

            var serviceResponse = await _chatHubServices.SendMessage(messageDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            var serviceResponse = await _chatHubServices.GetMessagesForConversation(conversationId);
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
