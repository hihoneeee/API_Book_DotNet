namespace TestWebAPI.DTOs.Evaluate
{
    public class GetEvaluateDTO
    {        
        public int id { get; set; }
        public int buyerId { get; set; }
        public int contractId { get; set; }
        public int star { get; set; }
        public string review { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }
}
