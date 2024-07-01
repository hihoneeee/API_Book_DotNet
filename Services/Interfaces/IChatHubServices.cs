using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IChatHubServices
    {
        Task<ServiceResponse<ConversationDTO>> GetOrCreateConversation(ConversationDTO createConversationDTO);
        Task<ServiceResponse<MessageDTO>> SendMessage(MessageDTO messageDTO);        
        Task JoinConversation(int conversationId, string connectionId);
        Task LeaveConversation(int conversationId, string connectionId);
        Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId);
    }
}
