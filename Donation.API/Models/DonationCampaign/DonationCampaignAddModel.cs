using System;

namespace Donation.API.Models.DonationCampaign
{
    public class DonationCampaignAddModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal TargetValue { get; set; }
    }
}
