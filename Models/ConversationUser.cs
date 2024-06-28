using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class ConversationUser
    {
        [Required]
        public int conversationId { get; set; }
        [ForeignKey("conversationId")]
        public Conversation Conversation { get; set; }
        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
    }
}
