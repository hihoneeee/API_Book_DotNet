using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.ChatHub
{
    public class ConversationDTO
    {
        [Required]
        public int userId1 { get; set; }
        [Required]
        public int userId2 { get; set; }
        public DateTime createdAt { get; set; }
        public List<GetMessageDTO> Messages { get; set; } = new List<GetMessageDTO>();
    }
}
