namespace TestWebAPI.Services.Interfaces
{
    public interface ISendMailService
    {
        Task SendEmailAsync(string to, string subject, string html);
    }
}
