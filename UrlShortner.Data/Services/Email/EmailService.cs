using Microsoft.Extensions.Options;
using MimeKit;
using UrlShortner.Data.Services.Email.Config;
using UrlShortner.Data.Services.Email.EmailModel;

namespace UrlShortner.Data.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendMail(EmailMessage message)
        {
            var emailMsg = CreateMessage(message);
            await Send(emailMsg);
        }

        private MimeMessage CreateMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("UrlShortner", _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(null, message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(MimeMessage msg)
        {
            var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await client.SendAsync(msg);
            }
            catch
            {
                throw new InvalidOperationException();
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
