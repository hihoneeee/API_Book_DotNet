using TestWebAPI.DTOs.Common;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Response
{
    public class ServiceResponse<T>
    {
        public string message { get; set; } = null;
        public T data { get; set; }
        public EHttpType statusCode { get; set; }
        public bool success { get; set; }
        public CommonResponseMsgDTO getMessage()
        {
            return new CommonResponseMsgDTO()
            {
                success = success,
                statusCode = (int)statusCode,
                message = message
            };
        }

        public CommonResponseDataDTO<T> getData()
        {
            return new CommonResponseDataDTO<T>()
            {
                statusCode = (int)statusCode,
                success = success,
                data = data
            };
        }
    }
}
