using System;

namespace Donation.API.Models.DonationCampaign
{
    public class DonationCampaignUpdateModel
    {
        public string Hash { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal TargetValue { get; set; }
    }
}
