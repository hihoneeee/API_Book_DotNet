using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetMessageDTO
    {
        [Required]
        public string content { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public int conversationId { get; set; }
        public DateTime createdAt { get; set; }
    }

}
