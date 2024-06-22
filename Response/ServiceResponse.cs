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
                message = message,
                data = data
            };
        }

        public CommonResponseAccessTokenDTO getAccessToken()
        {
            return new CommonResponseAccessTokenDTO()
            {
                success = success,
                message = message,
                statusCode = (int)statusCode,
                accessToken = accessToken
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
