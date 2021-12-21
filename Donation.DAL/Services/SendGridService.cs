using Donation.Infrastructure.Contract;
using Donation.Infrastructure.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Donation.Infrastructure.Services
{
    public class SendGridService : IEmailService
    {
        private readonly EmailSettings settings;
        private readonly ILogger<SendGridService> logger;

        public SendGridService(IOptions<EmailSettings> settings, ILogger<SendGridService> logger)
        {
            this.logger = logger;
            this.settings = settings.Value;
        }

        public async Task Send(string email, string name, string subject, string message, string template)
        {
            try
            {
                SendGridClient client = new SendGridClient(settings.ApiKey);

                EmailAddress from = new EmailAddress(settings.From, settings.Domain);

                EmailAddress to = new EmailAddress(email, name);

                SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, message, template);

                Response response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error ocurred to send an email to {name}-{email}");
            }
        }
    }
}