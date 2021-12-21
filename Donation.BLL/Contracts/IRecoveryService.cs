using Donation.Application.DataTransfer;
using Donation.Domain.Entities;
using System.Threading.Tasks;

namespace Donation.Application.Contracts
{
    public interface IRecoveryService
    {
        public Task SendRecoveryCode(ChangePasswordDTO model);

        public Task<Recovery> ConfirmRecoveryCode(ChangePasswordDTO model);

        public Task<bool> ChangePassword(ChangePasswordDTO model);
    }
}