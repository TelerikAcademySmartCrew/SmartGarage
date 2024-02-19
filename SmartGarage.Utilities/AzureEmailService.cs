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

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            _ = await emailClient.SendAsync(WaitUntil.Completed, SenderEmail, toEmail, subject, body);
        }
    }
}
