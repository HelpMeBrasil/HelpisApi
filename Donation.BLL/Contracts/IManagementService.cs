using Donation.Application.DataTransfer;
using System.Threading.Tasks;

namespace Donation.Application.Contracts
{
    public interface IManagementService
    {
        public Task<bool> ChangePassword(ChangePasswordDTO model);
    }
}