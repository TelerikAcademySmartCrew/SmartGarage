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

                string to = "vencious.games@gmail.com";
                string from = "venci1983@gmail.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Using the new SMTP client.";
                message.Body = @"Using this new feature, you can send an email message from an application very easily.";
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                // Credentials are necessary if the server requires the client
                // to authenticate before it will send email on the client's behalf.
                client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);

                try
                {
                    client.SendCompleted += Client_SendCompleted;
                
                    client.SendAsync(message, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                        ex.ToString());
                }

            // ==================================================

            //var email = GenerateEmailMessage(new Message(toEmail, subject, body));
            //SmtpClient client = new SmtpClient();
            ////client.Host = emailConfig.From;
            ////client.UseDefaultCredentials = false;
            //client.Host = emailConfig.SmtpServer;
            //client.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);
            //client.Port = emailConfig.Port;
            //client.EnableSsl = true;
            //await client.SendMailAsync(new MailMessage(emailConfig.Username, toEmail));

            // ==================================================

            //// Create a MailMessage object
            //using (MailMessage mail = new MailMessage(emailConfig.Username, toEmail))
            //{
            //    try
            //    {
            //        mail.Subject = "Test Email";
            //        mail.IsBodyHtml = true;
            //        // Create a SmtpClient object
            //        SmtpClient smtpClient = new SmtpClient();
            //        smtpClient.Host = emailConfig.SmtpServer;
            //        smtpClient.UseDefaultCredentials = false;
            //        smtpClient.Port = emailConfig.Port; // Port number for SMTP (587 for TLS/STARTTLS)
            //        smtpClient.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);
            //        smtpClient.EnableSsl = true; // Enable SSL/TLS encryption

            //        // Send the email
            //        smtpClient.Send(mail);
            //        Console.WriteLine("Email sent successfully.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Failed to send email: {ex.Message}");
            //    }
            //}

            // ==================================================

            //var email = GenerateEmail(new Message(toEmail, subject, body));

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            //var client = new MailKit.Net.Smtp.SmtpClient();
            ////client.CheckCertificateRevocation = false;
            ////client.AuthenticationMechanisms.Remove("XOAUTH2");
            //await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.StartTls);
            //await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);

            //await client.SendAsync(email);
            //await client.DisconnectAsync(true);
        }

        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            // Create an instance of the SmtpClient class
            using (SmtpClient smtpClient = new SmtpClient())
            {
                // Use the settings from web.config
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("venci1983@gmail.com", "kqhx fhbc lowr fljc");
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                // Create a MailMessage object
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("venci1983@gmail.com");
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                // Send the email
                smtpClient.Send(mailMessage);
            }
        }

        //private MailMessage GenerateEmailMessage(Message message)
        //{
        //    var emailMessage = new MailMessage();

        //    emailMessage.From = new MailAddress(emailConfig.From, "mail");
        //    emailMessage.To.Add(new MailAddress("vencious.games@gmail.com", "mail"));
        //    emailMessage.Subject = message.Subject;
        //    emailMessage.Body = message.Body;
        //    emailMessage.IsBodyHtml = false;

        //    return emailMessage;
        //}

        //private MimeMessage GenerateEmail(Message message)
        //{
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(MailboxAddress.Parse(emailConfig.From));
        //    emailMessage.To.Add(MailboxAddress.Parse(message.To.Address));
        //    emailMessage.Subject = message.Subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
        //    {
        //        Text = message.Body
        //    };

        //    return emailMessage;
        //}

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
