using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Threading.Tasks;
using TestWebAPI.Response;
using static TestWebAPI.Response.HttpStatus;

public class ErrorHandlingToken
{
    private readonly RequestDelegate _next;

    public ErrorHandlingToken(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SecurityTokenException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            // Tạo một đối tượng ServiceResponse chứa thông tin lỗi
            var serviceResponse = new ServiceResponse<object>
            {
                success = false,
                statusCode = EHttpType.Unauthorized,
                message = "Invalid access token!"
            };

            // Gửi phản hồi JSON chứa thông tin lỗi đến client
            await context.Response.WriteAsJsonAsync(serviceResponse.getMessage());
        }
    }
}
