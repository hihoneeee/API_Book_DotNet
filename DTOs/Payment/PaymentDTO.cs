using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWebAPI.DTOs.Payment
{
    public class PaymentDTO
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int contractId { get; set; }
        [Required]
        public string paypalId { get; set; }
        [Required]
        public string status { get; set; }
        [JsonIgnore]
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
