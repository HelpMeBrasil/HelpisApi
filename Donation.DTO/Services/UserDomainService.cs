using Donation.Domain.Contract;
using Donation.Domain.Contracts;
using Donation.Domain.Entities;
using Donation.Domain.Enumerators;
using Donation.Domain.Notifications;
using Donation.Domain.ValueObjects;
using System;

namespace Donation.Domain.Services
{
    public class UserDomainService : IUserDomainService
    {
        private readonly INotification notification;

        public UserDomainService(INotification notification)
        {
            this.notification = notification;
        }

        public bool UpdateUserPassword(User user, string oldPassword, string password)
        {
            if (user.Password.ValidatePassword(oldPassword) && !string.IsNullOrWhiteSpace(password))
            {
                user.Password = new Password(password);

                return true;
            }

            notification.AddNotification(new Notification(MappedErrors.ChangePasswordUnmatch));

            return false;
        }

        public bool UpdateUserPassword(User user, Guid recoveryId, string password)
        {
            int index = user.Recoveries.FindIndex(e => e.Identifier == recoveryId.ToString() && e.Active && e.Verified);

            if (user is not null && index > -1 && !string.IsNullOrWhiteSpace(password))
            {
                user.Password = new ValueObjects.Password(password);

                user.Recoveries[index].Inactive();

                return true;
            }

            notification.AddNotification(new Notification(MappedErrors.VerificationCodeExpired));

            return false;
        }

        public bool ValidateUserSignIn(User user, string password)
        {
            // Password validation
            if (user is null || string.IsNullOrWhiteSpace(password) || !user.Password.ValidatePassword(password))
            {
                notification.AddNotification(new Notification(MappedErrors.UsernameOrPasswordIncorrect));

                return false;
            }

            // User blocked
            if (user.Blocked)
            {
                notification.AddNotification(new Notification(MappedErrors.UserBlocked));
            }

            // User inactive
            if (!user.Active)
            {
                notification.AddNotification(new Notification(MappedErrors.UserInactive));
            }

            return !notification.HasNotifications();
        }

        public Recovery ValidateRecoveryCode(User user, string code)
        {
            int index = user.Recoveries.FindIndex(e => e.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && e.Active);

            if (index < 0 || user.Recoveries[index].Verified)
            {
                // if it does not exists rise a notification
                notification.AddNotification(new Notification(MappedErrors.VerificationCodeUnmatch));
            }
            else if (user.Recoveries[index].ExpiresOn <= DateTime.UtcNow)
            {
                // verify if it not was expired yet
                notification.AddNotification(new Notification(MappedErrors.VerificationCodeExpired));
            }

            if (notification.HasNotifications())
                return default;

            // set item as verified
            user.Recoveries[index].Verify();

            // Return the verification
            return user.Recoveries[index];
        }
    }
}