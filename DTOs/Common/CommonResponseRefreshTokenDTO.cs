namespace TestWebAPI.DTOs.Common
{
    public class CommonResponseRefreshTokenDTO
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string refreshToken { get; set; }
    }
}
