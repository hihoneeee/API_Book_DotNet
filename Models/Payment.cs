using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Payment
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int contractId { get; set; }
        [ForeignKey("contractId")]
        public virtual Contract contract { get; set; }
        [Required]
        public string paypalId { get; set; }
        [Required]
        public string status { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

    }
}
