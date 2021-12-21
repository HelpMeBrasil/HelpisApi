using Donation.Application.Transfers;
using Safe2Pay.Models;

namespace Donation.Application.Contracts
{
    public interface IUserService
    {
        public UserDTO Get(string id);

        public void Add(UserDTO user);

        public void Update(UserDTO user, Merchant merchant);
    }
}