using Donation.Domain.Helpers;
using System;

namespace Donation.Domain.Entities
{
    [BsonCollection("Recovery")]
    public class Recovery : Entity
    {
        public Recovery()
        {
            Identifier = Guid.NewGuid().ToString();
            Verified = false;
            Active = true;
            CreatedOn = DateTime.UtcNow;
            ExpiresOn = CreatedOn.AddMinutes(15);
            Code = new Random().Next(0, 1000000).ToString("D6");
        }

        public void Verify()
        {
            Verified = true;
            VerifiedOn = DateTime.UtcNow;
        }

        public void Inactive()
        {
            Active = false;
        }

        public string Identifier { get; set; }
        public string Code { get; set; }
        public bool Verified { get; set; }
        public bool Active { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime VerifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public User User { get; set; }
    }
}