using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IConversationServices
    {
        Task<ServiceResponse<List<GetConversationDTO>>> GetConversationByUserId(int userId);
    }
}
