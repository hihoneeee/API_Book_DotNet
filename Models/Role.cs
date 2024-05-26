namespace TestWebAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string code { get; set; }
        public required string value { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
    }
}
