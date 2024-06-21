using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Wishlist
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int buyer_id { get; set; }
        [ForeignKey("buyer_id")]
        public virtual User buyer { get; set; }
        [Required]
        public int property_id { get; set; }
        [ForeignKey("property_id")]
        public virtual Property property { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
