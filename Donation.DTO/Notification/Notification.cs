using Donation.Domain.Enumerators;
using Donation.Domain.Extensions;

namespace Donation.Domain.Notifications
{
    public class Notification
    {
        public string Key { get; }
        public string Message { get; }
        public string Code { get; }

        public Notification(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public Notification(MappedErrors error)
        {
            Key = error.Name();
            Message = error.Description();
            Code = error.ValueAsInt().ToString();
        }
    }
}