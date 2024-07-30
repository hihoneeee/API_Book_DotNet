using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRealTimeServices
    {
        Task<ServiceResponse<GetConversationDTO>> GetOrCreateConversation(int senderId, int receiverId);
        Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId);
        Task<ServiceResponse<GetNotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId);
    }
}
