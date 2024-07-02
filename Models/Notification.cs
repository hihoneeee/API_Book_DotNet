using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Notification
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        [Required]
        public int propertyId { get; set; }
        [ForeignKey("propertyId")]
        public virtual Property property { get; set; }
        [Required]
        public int conversationId { get; set; }
        [ForeignKey("conversationId")]
        public virtual Conversation conversation { get; set; }
        [Required]
        public string content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
