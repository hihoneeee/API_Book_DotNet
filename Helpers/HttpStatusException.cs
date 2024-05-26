namespace TestWebAPI.Helpers
{
    public class HttpStatusException : Exception
    {
        public int _statusCode { get; }
        public HttpStatusException(int statusCode, string message) : base(message)
        {
            _statusCode = statusCode;
        }
    }
}
