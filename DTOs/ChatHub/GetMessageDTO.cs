using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetMessageDTO
    {
        public string content { get; set; }
        public int userId { get; set; }
        public int conversationId { get; set; }
        public DateTime createdAt { get; set; }
    }

}
