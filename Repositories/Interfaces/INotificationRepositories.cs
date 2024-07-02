using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface INotificationRepositories
    {
        Task<List<Notification>> GetNotificationsForUser(int userId);
        Task MarkNotificationsAsRead(int userId);
        Task<Notification> CreateNoficationsAsync(Notification notification);

    }
}
