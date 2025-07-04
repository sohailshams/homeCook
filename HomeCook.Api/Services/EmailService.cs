
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HomeCook.Api.Services
{
    public class EmailService : IEmailService 
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> SendEmailAsync(string from_email, string to_email, string subject, string plainTextContent, string htmlContent)
        {
            var sendGridApiKey = _config["SendGrid:SendGridKey"];
            var client = new SendGridClient(sendGridApiKey);
            var from = new EmailAddress(from_email, "Home Cook Food Quantity");
            var to = new EmailAddress(to_email, "Home Cook Admin");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            return response.IsSuccessStatusCode;
        }
    }
}
