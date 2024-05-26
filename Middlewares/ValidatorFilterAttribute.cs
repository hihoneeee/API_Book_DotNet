using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static TestWebAPI.Response.HttpStatus;
using TestWebAPI.Response;

namespace TestWebAPI.Middlewares
{
    public class ValidatorFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                var errorMessage = string.Join("; ", errors);

                var serviceResponse = new ServiceResponse<object>
                {
                    statusCode = EHttpType.BadRequest,
                    success = false,
                    message = errorMessage
                };

                context.Result = new BadRequestObjectResult(serviceResponse.getMessage());
            }
        }
    }
}
