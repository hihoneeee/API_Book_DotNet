using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.DTOs.Contract
{
    public class GetContractDTO
    {
        public int id { get; set; }
        public string signatureBuyer { get; set; }
        public string signatureSeller { get; set; }
        public string content { get; set; }
        public int appointmentId { get; set; }
        public int EarnestMoney { get; set; }
        public DateTime createdAt { get; set; }
    }
}
