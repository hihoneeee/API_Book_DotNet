using Microsoft.AspNetCore.Authorization;
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
        private readonly IRealTimeServices _realTimeServices;

        public messagesController(IRealTimeServices realTimeServices)
        {
            _realTimeServices = realTimeServices;
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            var serviceResponse = await _realTimeServices.GetMessagesForConversation(conversationId);
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
