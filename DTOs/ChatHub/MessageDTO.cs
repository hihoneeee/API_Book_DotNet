using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class MessageDTO
    {
        [Required]
        public string content { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int recipientId { get; set; }
        [Required]
        public int conversationId { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
