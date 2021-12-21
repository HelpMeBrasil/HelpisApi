using Donation.Domain.Contracts;
using Donation.Domain.Enumerators;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Donation.Domain.Notifications
{
    public class NotificationContext : INotification
    {
        private readonly List<Notification> notifications;

        [JsonIgnore]
        public IReadOnlyCollection<Notification> Notifications => notifications;

        public NotificationContext()
        {
            notifications = new List<Notification>();
        }

        public void AddNotification(Notification notification)
        {
            notifications.Add(notification);
        }

        public void AddNotification(MappedErrors mapped)
        {
            notifications.Add(new Notification(mapped));
        }

        public bool HasNotifications()
        {
            return notifications.Count > 0;
        }

        public IList<Notification> GetNotifications()
        {
            return notifications;
        }
    }
}