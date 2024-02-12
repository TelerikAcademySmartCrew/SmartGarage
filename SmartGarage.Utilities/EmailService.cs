//using System.Net;
//using System.Net.Mail;

using MailKit.Security;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Org.BouncyCastle.Utilities.Net;
using SmartGarage.Utilities.Models;
using System.Net;
using System.Net.Mail;

namespace SmartGarage.Utilities
{
    public class EmailService
    {
        private readonly EmailConfig emailConfig;

        public EmailService(EmailConfig emailConfig)
        {
            this.emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
                var from = emailConfig.Username;
                var message = new MailMessage(from, toEmail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                
                var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                message.AlternateViews.Add(htmlView);

                message.Body = body;
                var client = new SmtpClient(emailConfig.SmtpServer, emailConfig.Port);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);

                try
                {
                    client.SendAsync(message, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                        ex.ToString());
                }
        }
        
    }
}
