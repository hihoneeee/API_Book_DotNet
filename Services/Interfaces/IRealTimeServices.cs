using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IRealTimeServices
    {
        Task<ServiceResponse<GetConversationDTO>> GetOrCreateConversation(ConversationDTO conversationDTO);
        Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId);
        Task<ServiceResponse<GetNotificationDTO>> CreateAppointmentNotificationAsync(int propertyId, int buyerId);
    }
}
