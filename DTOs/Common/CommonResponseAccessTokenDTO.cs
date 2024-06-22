namespace TestWebAPI.DTOs.Common
{
    public class CommonResponseAccessTokenDTO
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string accessToken { get; set; }

    }
}
