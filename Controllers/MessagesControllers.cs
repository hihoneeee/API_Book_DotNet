using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class messagesController : ControllerBase
    {
        private readonly IChatHubRepositories _chatHubRepo;

        public messagesController(IChatHubRepositories chatHubRepo)
        {
            _chatHubRepo = chatHubRepo;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            await _chatHubRepo.SendMessage(messageDTO.conversationId, messageDTO.userId, messageDTO.content);
            return Ok(messageDTO);
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            var messages = await _chatHubRepo.GetMessagesForConversation(conversationId);
            return Ok(messages);
        }
    }
}
