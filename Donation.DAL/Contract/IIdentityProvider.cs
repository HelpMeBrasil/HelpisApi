using Donation.Domain.Entities;

namespace Donation.Infrastructure.Contract
{
    public interface IIdentityProvider
    {
        public string Id();

        public string Username();

        public Token SignIn(User user);
    }
}