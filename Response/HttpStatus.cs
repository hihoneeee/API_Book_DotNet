namespace TestWebAPI.Response
{
    public class HttpStatus
    {
        public enum EHttpType { 
            Success = 200,
            NotFound = 404,
            CannotCreate = 400,
            CannotUpdate = 400,
            CannotDelete = 400,
            Created = 201,
            Forbid = 403,
            BadRequest = 400,
            Unauthorized = 401,
            InternalError = 500
        }
    }
}
