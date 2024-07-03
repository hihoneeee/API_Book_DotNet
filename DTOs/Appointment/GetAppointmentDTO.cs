using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Appointment
{
    public class GetAppointmentDTO
    {
        [Required]
        public int id { get; set; }
        public int buyerId { get; set; }
        [Required]
        public int propertyId { get; set; }
        [Required]
        public DateTime appointmentDate { get; set; }
        [Required]
        public DateTime backupDay { get; set; }
        [Required]
        public EnumStatus status { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
    }
}
