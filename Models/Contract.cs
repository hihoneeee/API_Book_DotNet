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
        public string content { get; set; }
        [Required]
        public int appointment_id { get; set; }
        [ForeignKey("appointment_id")]
        public virtual Appointment appointment { get; set; }
        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
