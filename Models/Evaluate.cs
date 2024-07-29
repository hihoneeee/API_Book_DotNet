using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Models
{
    public class Evaluate
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int buyerId { get; set; }
        public int contractId { get; set; }
        [ForeignKey("contractId")]
        public virtual Contract contract { get; set; }
        [Required]
        public int star { get; set; }
        [Required]
        public string review { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; }
    }
}
