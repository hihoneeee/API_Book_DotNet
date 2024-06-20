using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Response
{
    public interface IServiceResponse
    {
        bool Success { get; set; }
        string Message { get; set; }
        EHttpType StatusCode { get; set; }
    }
}
