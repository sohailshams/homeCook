namespace HomeCook.Api.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string from_email, string to_email, string subject, string plainTextContent, string htmlContent);
    }
}
