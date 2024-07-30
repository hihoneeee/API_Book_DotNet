using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Message
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
        [Required]
        public int conversationId { get; set; }
        [ForeignKey("conversationId")]
        public Conversation Conversation { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; }
    }
}
