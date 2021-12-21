using Donation.Application.Transfers;
using System.Threading.Tasks;

namespace Donation.Application.Contracts
{
    public interface IIdentityService
    {
        public Task<IdentityDTO> SignInAsync(AuthDTO auth);
    }
}