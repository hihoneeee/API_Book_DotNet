using static TestWebAPI.Response.HttpStatus;
using TestWebAPI.Response;

namespace TestWebAPI.Helpers
{
    public static class ResponseHelper
    {
        public static void SetNotFound(this IServiceResponse response, string message)
        {
            response.Success = false;
            response.Message = $"{message} does not found!";
            response.StatusCode = EHttpType.NotFound;
        }

        public static void SetUnauthorized(this IServiceResponse response, string message)
        {
            response.Success = false;
            response.Message = message;
            response.StatusCode = EHttpType.NotFound;
        }

        public static void SetBadRequest(this IServiceResponse response, string message)
        {
            response.Success = false;
            response.Message = message;
            response.StatusCode = EHttpType.BadRequest;
        }

        public static void SetExisting(this IServiceResponse response, string message)
        {
            response.Success = false;
            response.Message = $"{message} already exists!";
            response.StatusCode = EHttpType.Conflict;
        }

        public static void SetSuccess(this IServiceResponse response, string message)
        {
            response.Success = true;
            response.Message = message;
            response.StatusCode = EHttpType.Success;
        }

        public static void SetError(this IServiceResponse response, string message, EHttpType statusCode = EHttpType.InternalError)
        {
            response.Success = false;
            response.Message = $"An unexpected error occurred: {message}";
            response.StatusCode = statusCode;
        }
    }
}
