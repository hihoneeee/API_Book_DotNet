using TestWebAPI.DTOs.Common;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Response
{
    public class ServiceResponse<T> : IServiceResponse
    {
        public string message { get; set; }
        public T data { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public EHttpType statusCode { get; set; }
        public bool success { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }

        public CommonResponseMsgDTO getMessage()
        {
            return new CommonResponseMsgDTO()
            {
                success = success,
                message = message
            };
        }



        bool IServiceResponse.Success
        {
            get => success;
            set => success = value;
        }

        string IServiceResponse.Message
        {
            get => message;
            set => message = value;
        }

        EHttpType IServiceResponse.StatusCode
        {
            get => statusCode;
            set => statusCode = value;
        }
    }
}
