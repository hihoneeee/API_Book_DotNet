using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Conversation
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public virtual ICollection<Message> messages { get; set; }
        public virtual ICollection<ConversationUser> conversationUsers { get; set; }
    }
}
