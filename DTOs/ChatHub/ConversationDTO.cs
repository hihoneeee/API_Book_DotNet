using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.User;

namespace TestWebAPI.DTOs.ChatHub
{
    public class ConversationDTO
    {
        [Required]
        public int userId1 { get; set; }
        [Required]
        public int userId2 { get; set; }
        public DateTime updatedAt { get; set; } = DateTime.Now;
        public virtual GetUserDTO dataReceiver { get; set; }

    }
}
