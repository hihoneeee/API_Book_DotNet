using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Contract
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string signature_buyer { get; set; }
        [Required]
        public string signature_seller { get; set; }
        [Required]
        public int buyer_id { get; set; }
        [ForeignKey("buyer_id")]
        public virtual User buyer { get; set; }
        [Required]
        public int property_id { get; set; }
        [ForeignKey("property_id")]
        public virtual Property property { get; set; }
        public DateTime transaction_date { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;

    }
}
