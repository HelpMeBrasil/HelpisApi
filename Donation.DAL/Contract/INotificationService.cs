using Donation.Domain.Entities;
using Donation.Domain.Enumerators;
using System.Threading.Tasks;

namespace Donation.Application.Contracts
{
    public interface INotificationService
    {
        public Task SendVerificationCode(User user, string code, NotificationType type);
    }
}