using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetMessageDTO
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
