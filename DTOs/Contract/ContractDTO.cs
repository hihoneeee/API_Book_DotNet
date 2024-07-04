using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Contract
{
    public class ContractDTO
    {
        [Required]
        public string signatureBuyer { get; set; }
        [Required]
        public string signatureSeller { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public int appointmentId { get; set; }
        [Required]
        public int EarnestMoney { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
