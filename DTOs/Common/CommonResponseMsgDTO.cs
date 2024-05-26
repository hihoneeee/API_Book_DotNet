namespace TestWebAPI.DTOs.Common
{
    public class CommonResponseMsgDTO
    {
        public string message { get; set; }
        public int statusCode { get; set; }
        public bool success { get; set; }
    }
}
