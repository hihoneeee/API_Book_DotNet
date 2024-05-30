namespace TestWebAPI.DTOs.Common
{
    public class CommonResponseMsgDTO
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
