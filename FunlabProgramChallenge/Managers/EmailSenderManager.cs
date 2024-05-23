using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.Utility;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FunlabProgramChallenge.Managers
{
    public class EmailSenderManager : IEmailSenderManager
    {
        private readonly ILogger<EmailSenderManager> _iLogger;
        private readonly EmailConfig _emailConfig;

        public EmailSenderManager(ILogger<EmailSenderManager> iLogger, IOptions<EmailConfig> emailConfig)
        {
            _iLogger = iLogger;
            _emailConfig = emailConfig.Value;
        }

        public async Task<Result> SendEmailMessage(EmailMessage emailMessage)
        {
            Result result = new Result();
            try
            {
                var smtp = new SmtpClient
                {
                    Host = _emailConfig.Host.ToString(),
                    Port = Convert.ToInt32(_emailConfig.Port),
                    EnableSsl = Convert.ToBoolean(_emailConfig.EnableSsl),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailConfig.FromEmailAddress, _emailConfig.Password)
                };
                using (var smtpMessage = new MailMessage(new MailAddress(_emailConfig.FromEmailAddress, _emailConfig.DisplayName), new MailAddress(emailMessage.ReceiverEmail, emailMessage.ReceiverName)))
                {
                    smtpMessage.Subject = emailMessage.Subject;
                    smtpMessage.Body = emailMessage.Body;
                    smtpMessage.IsBodyHtml = emailMessage.IsHtml;
                    smtpMessage.Priority = MailPriority.High;
                    smtpMessage.SubjectEncoding = Encoding.UTF8;
                    smtpMessage.BodyEncoding = Encoding.UTF8;
                    smtpMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                    await smtp.SendMailAsync(smtpMessage);

                    smtp.Dispose();

                    _iLogger.LogInformation("EmailSenderManager - SendEmailMessage - Email send successfully for email: " + emailMessage.ReceiverEmail);

                    result = Result.Ok(MessageHelper.SentMessage, parentId: emailMessage.ReceiverEmail, parentName: emailMessage.ReceiverName);

                }
            }
            catch (Exception ex)
            {
                result = Result.Fail(MessageHelper.SentMessageFail);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "SendEmailMessage", emailMessage.ReceiverEmail));
            }

            return result;
        }

    }

    public interface IEmailSenderManager
    {
        Task<Result> SendEmailMessage(EmailMessage emailMessage);
    }
}
