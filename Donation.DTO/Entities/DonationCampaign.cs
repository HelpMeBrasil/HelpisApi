using Donation.Domain.Entities;
using Donation.Domain.Helpers;
using System;

namespace Donation.DTO.Entities
{
    [BsonCollection("DonationCampaign")]
    public class DonationCampaign : Entity
    {
        public User User { get; set; }
        public string Hash { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal TargetValue { get; set; }

        public DonationCampaign(User user, string hash, bool isActive,
                                string title, string description, string image, decimal targetValue)
        {
            User = user;
            Hash = hash;
            IsActive = true;
            Title = title;
            Description = description;
            Image = image;
            TargetValue = targetValue;
        }

        public void UpdateDonationCampaign(User user, string hash, bool isActive,
                                           string title, string description, string image, decimal targetValue)
        {
            User = user;
            Hash = hash;
            IsActive = isActive;
            Title = title;
            Description = description;
            Image = image;
            TargetValue = targetValue;
        }
    }
}
