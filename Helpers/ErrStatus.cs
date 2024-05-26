namespace TestWebAPI.Helpers
{
        public class ErrStatus
        {
            public bool Success { get; }
            public int StatusCode { get; }
            public string Message { get; }

            public ErrStatus(bool success, int statusCode, string message)
            {
                Success = success;
                StatusCode = statusCode;
                Message = message;
            }
        }

 
}
