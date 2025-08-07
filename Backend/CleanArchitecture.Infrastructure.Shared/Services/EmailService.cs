using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.DTOs.Email;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CleanArchitecture.Infrastructure.Shared.Services
{
    public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        : IEmailService
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        public void Dispose()
        {
        }
    }
}
