namespace TestWebAPI.DTOs.Common
{
    public class CommonResponseDataDTO<T>
    {
        public T data { get; set; }
        public int statusCode { get; set; }
        public bool success { get; set; }
    }
}
