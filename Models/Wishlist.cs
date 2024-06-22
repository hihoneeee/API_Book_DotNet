using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Wishlist
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int buyerId { get; set; }
        [ForeignKey("buyerId")]
        public virtual User buyer { get; set; }
        [Required]
        public int propertyId { get; set; }
        [ForeignKey("propertyId")]
        public virtual Property property { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
