namespace TestWebAPI.DTOs.User
{
    public class GetUserMediaDTO
    {
        public int id { get; set; }
        public string provider { get; set; }
        public string icon { get; set; }
        public string link { get; set; }
    }
}
