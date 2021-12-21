using System.Threading.Tasks;

namespace Donation.Infrastructure.Contract
{
    public interface IEmailService
    {
        public Task Send(string email, string name, string subject, string message, string template);
    }
}