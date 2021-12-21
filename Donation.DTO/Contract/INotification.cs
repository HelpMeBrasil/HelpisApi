using Donation.Domain.Enumerators;
using Donation.Domain.Notifications;
using System.Collections.Generic;

namespace Donation.Domain.Contracts
{
    public interface INotification
    {
        public void AddNotification(Notification notification);

        public void AddNotification(MappedErrors notification);

        public bool HasNotifications();

        public IList<Notification> GetNotifications();
    }
}