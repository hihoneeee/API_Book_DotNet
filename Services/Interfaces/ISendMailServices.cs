namespace TestWebAPI.Services.Interfaces
{
    public interface ISendMailServices
    {
        Task SendEmailAsync(string to, string subject, string html);
    }
}
