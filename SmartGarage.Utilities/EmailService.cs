//using System.Net;
//using System.Net.Mail;

using MailKit.Security;
using MimeKit;
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
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("FromName", "fromAddress@gmail.com"));
            //message.To.Add(new MailboxAddress("", toEmail));
            //message.Subject = subject;
            //message.Body = new TextPart("plain") { Text = body };

            //using (var client = new SmtpClient())
            //{
            //    //client.ServerCertificateValidationCallback = (object sender,
            //    //    X509Certificate certificate,
            //    //    X509Chain chain,
            //    //    SslPolicyErrors sslPolicyErrors) => true;

            //    client.CheckCertificateRevocation = false;
            //    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.None);
            //    await client.AuthenticateAsync(emailConfig.From, emailConfig.Password);
            //    await client.SendAsync(message);
            //    await client.DisconnectAsync(true);
            //}

            // =====

            //{

            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            //    var mailMessage = new MimeMessage();
            //    mailMessage.From.Add(new MailboxAddress("from", emailConfig.From));
            //    mailMessage.To.Add(new MailboxAddress("to", toEmail));
            //    mailMessage.Subject = subject;
            //    mailMessage.Body = new TextPart(TextFormat.Plain)
            //    {
            //        Text = body
            //    };
            //    using (var client = new MailKit.Net.Smtp.SmtpClient())
            //    {
            //        client.CheckCertificateRevocation = false;
            //        await client.ConnectAsync(emailConfig.SmtpServer, 587, false);
            //        await client.AuthenticateAsync(emailConfig.From, emailConfig.Password);
            //        client.Send(mailMessage);
            //        client.Disconnect(true);
            //    }
            //}

            // ==================================================

            //var email = GenerateEmailMessage(new Message(toEmail, subject, body));
            //SmtpClient client = new SmtpClient();
            ////client.Host = emailConfig.From;
            //client.UseDefaultCredentials = false;
            //client.Host = emailConfig.SmtpServer;
            //client.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);
            //client.Port = emailConfig.Port;
            //await client.SendMailAsync(email);

            // ==================================================

            var email = GenerateEmail(new Message(toEmail, subject, body));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            var client = new MailKit.Net.Smtp.SmtpClient();
            //client.CheckCertificateRevocation = false;
            //client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);

            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }

        private MailMessage GenerateEmailMessage(Message message)
        {
            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress(emailConfig.From, "mail");
            emailMessage.To.Add(new MailAddress("vencious.games@gmail.com", "mail"));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = message.Body;
            emailMessage.IsBodyHtml = false;

            return emailMessage;
        }

        private MimeMessage GenerateEmail(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(message.To.Address));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Body
            };

            return emailMessage;
        }

        //public static async Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    var fromAddress = new MailAddress(ourEmail, "Smart Garage Team");
        //    var toAddress = new MailAddress(toEmail);
        //    

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com", // Use the SMTP server corresponding to your email provider
        //        Port = 587,
        //        EnableSsl = false,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
        //        Timeout = 20000,
        //    };

        //    using var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = false, // Set to true if your body contains HTML
        //    };

        //    await smtp.SendMailAsync(message);
        //}

        //public async Task SendMailToUser(string email, string userName, string passWord)
        //{
        //    //using (var client = new SmtpClient(smtpServer, smtpPort))
        //    //{
        //        var client = new SmtpClient(smtpServer, smtpPort);
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        //        client.EnableSsl = true;

        //        var message = new MailMessage
        //        {
        //            From = new MailAddress(ourEmail),
        //            Subject = "Welcome to SmartGarage. Please use the possword sent with this email to login.",
        //            Body = $"Hello {userName}.\n\nYour account has been created." +
        //            $"Please you the following password: {passWord} to login." +
        //            $"You will be able to fill out your user profile data upon login"
        //        };

        //        message.To.Add(email);

        //        await client.SendMailAsync(message);
        //    //}
        //}
    }
}
