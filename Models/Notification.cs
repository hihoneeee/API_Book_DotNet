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
        [Required]
        public string content { get; set; }
        [Required]
        public bool IsRead { get; set; } = false;
        [Required]
        public int buyerId { get; set; }
        [ForeignKey("buyerId")]
        public virtual User user { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; }
    }
}
