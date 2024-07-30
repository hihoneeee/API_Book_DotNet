using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetOrCreateConversation
    {
        [Required]
        public int receiverId { get; set; }
    }
}
