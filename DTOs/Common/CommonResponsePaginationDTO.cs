namespace TestWebAPI.DTOs.Common
{
    public class CommonResponsePaginationDTO
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int total { get; set; }
        public int limtit {  get; set; }
        public int page { get; set; }
    }
}
