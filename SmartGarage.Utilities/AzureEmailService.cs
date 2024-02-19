using System.Net.Mime;

using Azure;
using Azure.Communication.Email;

using SmartGarage.Utilities.Contract;

namespace SmartGarage.Utilities
{
    public class AzureEmailService : IEmailService
    {
        private const string SenderEmail = "DoNotReply@e5b418ff-9ee5-4fdc-b08f-e8bcf7bfc02c.azurecomm.net";

        private readonly EmailClient emailClient;

        public AzureEmailService(EmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, byte[] attactchment)
        {
            var emailContent = new EmailContent(subject)
            {
                Html = body
            };

            var emailMessage = new EmailMessage(SenderEmail, toEmail, emailContent);

            if (attactchment != null)
            {
                var binaryDataAttachment = new BinaryData(attactchment);
                var emailAttachment = new EmailAttachment("attachment.pdf", MediaTypeNames.Application.Pdf, binaryDataAttachment);

                emailMessage.Attachments.Add(emailAttachment);
            }

            _ = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
        }
    }
}
