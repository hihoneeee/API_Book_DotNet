using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public int buyerId { get; set; }
        [Required]
        public int propertyId { get; set; }
        [Required]
        public DateTime appointmentDate { get; set; }
        [Required]
        public DateTime backupDay { get; set; }
        [JsonIgnore]
        public EnumStatus status { get; set; } = EnumStatus.pending;
        [JsonIgnore]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
