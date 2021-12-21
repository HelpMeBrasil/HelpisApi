using Donation.Application.Contracts;
using Donation.Domain.Entities;
using Donation.Domain.Enumerators;
using Donation.Infrastructure.Contract;
using Donation.Infrastructure.Models;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace Donation.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService emailService;

        public NotificationService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task SendVerificationCode(User user, string code, NotificationType type)
        {
            try
            {
                ResourceSet resources = ResourceHelper.GetResources("pt-BR");

                string subject = resources.GetString("EmailRecoverySubject");

                // Get email template
                string template = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                                   "Resource/Templates/RecoveryEmailTemplate.html"));
                // Replace keys
                template = template.Replace("[#Name]", user.FirstName)
                                   .Replace("[#RecoveryCode]", code);

                //Send async
                await emailService.Send(user.Email, user.FirstName, subject, "", template);
            }
            catch (System.Exception e)
            {

                throw;
            }
            
        }
    }
}