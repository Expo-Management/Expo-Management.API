using SendGrid;
using SendGrid.Helpers.Mail;

namespace Expo_Management.API.Services
{
    public interface IMailService
    {

        Task SendEmailAsync(string toEmail, string templateId, object dynamicTemplateData);
    }

    public class SenderGridMailService : IMailService
    {

        private readonly IConfiguration _configuration;

        public SenderGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string templateId, object dynamicTemplateData)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreplyexpomanagement@gmail.com", "Expo-Management Team");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);

            var response = await client.SendEmailAsync(msg);
        }
    }

    public class DynamicTemplate
    {

    }
}
