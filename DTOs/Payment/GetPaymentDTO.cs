
namespace TestWebAPI.DTOs.Payment
{
    public class GetPaymentDTO
    {
        public int id { get; set; }
        public int contractId { get; set; }
        public string paypalId { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
    }
}
