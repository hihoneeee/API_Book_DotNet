using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Models;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRealTimeServices
    {
        Task<ServiceResponse<GetConversationDTO>> GetOrCreateConversation(ConversationDTO conversationDTO);
        Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId);
        Task<ServiceResponse<List<GetNotificationDTO>>> GetNotificationsForUser(int userId);
        Task<ServiceResponse<bool>> MarkNotificationsAsRead(int userId);
        Task<ServiceResponse<NotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId);

        // Join and Connect
        Task OnDisconnectedAsync(int id, string connectionId);
        Task OnConnectedAsync(int id, string connectionId);

    }
}
