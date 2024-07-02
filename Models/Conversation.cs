using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Conversation
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int userId1 { get; set; }
        [Required]
        public int userId2 { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

    }
}
