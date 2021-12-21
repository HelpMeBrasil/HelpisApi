using Donation.Domain.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Donation.Domain.Entities
{
    [BsonCollection("Token")]
    public class Token : Entity
    {
        public Token()
        { }

        public Token(User user, int accessExpiration)
        {
            User = user;
            CreateDate = DateTime.UtcNow;
            ExpiryDate = CreateDate.AddHours(accessExpiration);
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime RefreshDate { get; set; }
        public User User { get; set; }
    }
}