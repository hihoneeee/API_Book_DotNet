using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Models;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRealTimeServices
    {
        Task<ServiceResponse<GetConversationDTO>> GetOrCreateConversation(ConversationDTO conversationDTO);
        Task<ServiceResponse<MessageDTO>> SendMessage(MessageDTO messageDTO);        
        Task JoinConversation(int conversationId, string connectionId);
        Task LeaveConversation(int conversationId, string connectionId);
        Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId);
        Task<ServiceResponse<NotificationDTO>> CreateNotification(NotificationDTO notificationDTO);
        Task<ServiceResponse<List<GetNotificationDTO>>> GetNotificationsForUser(int userId);
        Task<ServiceResponse<bool>> MarkNotificationsAsRead(int userId);
    }
}
