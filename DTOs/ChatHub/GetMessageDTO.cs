using System.ComponentModel.DataAnnotations;
using TestWebAPI.DTOs.User;

namespace TestWebAPI.DTOs.ChatHub
{
    public class GetMessageDTO
    {
        public string content { get; set; }
        public bool IsRead { get; set; }
        public int userId { get; set; }
        public int conversationId { get; set; }
        public DateTime createdAt { get; set; }
        public virtual GetUserDTO dataUser { get; set; }
    }

}
