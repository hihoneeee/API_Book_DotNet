using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Notification
{
    public class NotificationDTO
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        [JsonIgnore]
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
