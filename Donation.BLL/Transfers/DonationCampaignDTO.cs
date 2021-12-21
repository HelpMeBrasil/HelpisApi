using Donation.Application.Transfers;
using Donation.Domain.Entities;
using System;

namespace Donation.BLL.Transfers
{
    public class DonationCampaignDTO
    {
        public string Id { get; set; }
        public UserDTO User { get; set; }
        public string Hash { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal TargetValue { get; set; }
    }
}
