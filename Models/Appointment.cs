using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum EnumStatus
{
    pending,
    rejected,
    accepted,
};


namespace TestWebAPI.Models
{
    public class Appointment
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
        public DateTime appointmentDate { get; set; }
        [Required]
        public DateTime backupDay { get; set; }
        [Required]
        public EnumStatus status { get; set; } = EnumStatus.pending;
        [Required]
        public DateTime createdAt { get; set;} = DateTime.Now;
        [Required]
        public DateTime updatedAt { get; set; }
        public virtual Contract contract { get; set; }
    }
}
