using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IChatHubRepositories
    {
        Task CreateConversation(string conversationName);
        Task SendMessage(int conversationId, int userId, string messageContent);
        Task<List<Message>> GetMessagesForConversation(int conversationId);
        Task JoinConversation(int conversationId, string connectionId);
        Task LeaveConversation(int conversationId, string connectionId);
    }
}
