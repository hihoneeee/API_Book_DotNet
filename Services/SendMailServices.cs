using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using TestWebAPI.DTOs.Common;
using TestWebAPI.Services.Interfaces;
using MailKit.Net.Smtp;
using TestWebAPI.Config;

namespace TestWebAPI.Services
{
    public class SendMailServices : ISendMailService
    {
        private readonly EmailConfiguration _emailConfig;

        public SendMailServices(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
