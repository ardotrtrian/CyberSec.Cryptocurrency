using CyberSec.Cryptocurrency.Service.Interfaces;
using CyberSec.Cryptocurrency.Service.Models.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CyberSec.Cryptocurrency.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _setting;

        public EmailService(IOptions<EmailSetting> emailSetting)
        {
            _setting = emailSetting.Value;
        }

        public async Task SendAsync(string to, string subject, string html, string from = null!)
        {
            //create message

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _setting.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            //send email

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_setting.SmtpHost, _setting.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_setting.SmtpUser, _setting.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
