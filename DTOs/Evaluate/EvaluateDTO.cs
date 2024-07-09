using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Evaluate
{
    public class EvaluateDTO
    {
        [Required]
        public int buyerId { get; set; }
        [Required]
        public int contractId { get; set; }
        [Required]
        public int star { get; set; }
        [Required]
        public string review { get; set; }

    }
}
