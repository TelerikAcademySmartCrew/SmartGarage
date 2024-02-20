namespace SmartGarage.Utilities.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[]? attactchment = null);
    }
}
