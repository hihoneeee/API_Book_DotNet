using TestWebAPI.DTOs.Notification;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface INotificationServices
    {
        Task<ServiceResponse<List<GetNotificationDTO>>> GetNotificationsForUser(int userId);
        Task<ServiceResponse<bool>> MarkNotificationsAsRead(int userId);
    }
}
