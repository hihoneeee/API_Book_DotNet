using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using TestWebAPI.Services.Interfaces;
using MailKit.Net.Smtp;
using TestWebAPI.Settings;

namespace TestWebAPI.Services
{
    public class SendMailServices : ISendMailServices
    {
        private readonly SendEmailSetting _sendEmailSetting;

        public SendMailServices(IOptions<SendEmailSetting> sendEmailSetting)
        {
            _sendEmailSetting = sendEmailSetting.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_sendEmailSetting.SenderName, _sendEmailSetting.SenderEmail));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_sendEmailSetting.SmtpServer, _sendEmailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_sendEmailSetting.Username, _sendEmailSetting.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
