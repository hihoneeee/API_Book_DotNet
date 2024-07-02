using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetConversationDTO
    {
        public int id { get; set; }
        [Required]
        public int userId1 { get; set; }
        [Required]
        public int userId2 { get; set; }
        public DateTime createdAt { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<GetMessageDTO> dataMessages { get; set; }
    }
}
