using Donation.Application.Transfers;
using Donation.BLL.Transfers;
using Donation.DTO.Entities;
using System;
using System.Collections.Generic;

namespace Donation.BLL.Contracts
{
    public interface IDonationCampaignService
    {
        public DonationCampaignDTO Get(string hash);
        public List<DonationCampaignDTO> ListSite(string campaignName);
        public List<DonationCampaignDTO> ListAllCampaign();
        public List<DonationCampaignDTO> ListGridByName(string campaignName, string idUser);
        public List<DonationCampaignDTO> ListGridByUserName(string userName);
        public string Add(DonationCampaignDTO campaign, UserDTO user);
        public void Update(DonationCampaignDTO campaign, UserDTO user);
        public void Delete(string hash, UserDTO user);
    }
}
