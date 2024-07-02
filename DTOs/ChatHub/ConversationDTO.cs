using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class ConversationDTO
    {
        [Required]
        public int userId1 { get; set; }
        [Required]
        public int userId2 { get; set; }
    }
}
