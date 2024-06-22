using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebAPI.Models
{
    public class Offer
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int appointmentId { get; set; }
        [ForeignKey("appointmentId")]
        public virtual Appointment appointment { get; set; }
        [Required]
        public EnumStatus status { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
        public virtual Contract Contract { get; set; } 
    }
}
