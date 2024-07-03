using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Notification
{
    public class GetNotificationDTO
    {
        public int id { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public string content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
