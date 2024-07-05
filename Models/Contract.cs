using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Contract
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string signatureBuyer { get; set; }
        [Required]
        public string signatureSeller { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public int appointmentId { get; set; }
        [ForeignKey("appointmentId")]
        public virtual Appointment appointment { get; set; }
        [Required]
        public int EarnestMoney { get; set; }
        [Required]
        public virtual Payment payment { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}
