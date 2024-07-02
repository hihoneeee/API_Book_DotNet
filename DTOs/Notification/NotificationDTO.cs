using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Notification
{
    public class NotificationDTO
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public int propertyId { get; set; }
        [Required]
        public int conversationId { get; set; }
        [Required]
        [JsonIgnore]
        public string content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
