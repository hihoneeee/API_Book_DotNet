using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class conversationsController : ControllerBase
    {
        private readonly IChatHubRepositories _chatHubRepo;

        public conversationsController(IChatHubRepositories chatHubRepo)
        {
            _chatHubRepo = chatHubRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDTO createConversationDTO)
        {
            await _chatHubRepo.CreateConversation(createConversationDTO.Name);
            return Ok();
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinConversation(int conversationId, string connectionId)
        {
            await _chatHubRepo.JoinConversation(conversationId, connectionId);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveConversation(int conversationId, string connectionId)
        {
            await _chatHubRepo.LeaveConversation(conversationId, connectionId);
            return Ok();
        }
    }
}
