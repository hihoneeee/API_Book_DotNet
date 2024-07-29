using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IChatHubRepositories
    {
        Task<Conversation> CheckkConversation(int userId1, int userId2);
        Task<Conversation> CreateConversation(Conversation conversation);
        Task<Conversation> GetConversationById(int id);   
        Task<Message> SendMessage(Message message);
        Task<List<Message>> GetMessagesForConversation(int conversationId);
        Task<List<Conversation>> GetConversationByUserId(int userId);
        Task<bool> CheckUserSendMessageAsync(int userId);
        Task<Conversation> UpdateConversationAsync(Conversation oldConversation);
    }
}
