using Donation.Domain.Entities;
using System;

namespace Donation.Domain.Contract
{
    public interface IUserDomainService
    {
        bool ValidateUserSignIn(User user, string password);

        bool UpdateUserPassword(User user, Guid recoveryId, string password);

        bool UpdateUserPassword(User user, string oldPassword, string password);

        public Recovery ValidateRecoveryCode(User user, string code);
    }
}