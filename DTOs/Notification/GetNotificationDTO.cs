using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Notification
{
    public class GetNotificationDTO
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int buyerId { get; set; }
        public string content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
