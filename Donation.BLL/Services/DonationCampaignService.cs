using Donation.Application.Transfers;
using Donation.BLL.Contracts;
using Donation.BLL.Transfers;
using Donation.Domain.Contracts;
using Donation.Domain.Entities;
using Donation.DTO.Entities;
using Donation.Infrastructure.Contract;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Donation.BLL.Services
{
    public class DonationCampaignService : IDonationCampaignService
    {
        private readonly IMongoRepository<DonationCampaign> repository;
        private readonly INotification notification;
        private readonly ILogger<DonationCampaignService> logger;

        public DonationCampaignService(IMongoRepository<DonationCampaign> repository,
                                       INotification notification, ILogger<DonationCampaignService> logger)
        {
            this.repository = repository;
            this.notification = notification;
            this.logger = logger;
        }
        public string Add(DonationCampaignDTO campaign, UserDTO user)
        {
            campaign.Hash = Guid.NewGuid().ToString();

            user.Password = null;
            user.MerchantSafe2Pay = null;

            campaign.User = user;

            DonationCampaign data = new DonationCampaign(campaign.User.Adapt<User>(), campaign.Hash, campaign.IsActive,
                campaign.Title, campaign.Description, campaign.Image, campaign.TargetValue);

            repository.InsertOne(data);

            return campaign.Hash;
        }

        public void Delete(string hash, UserDTO user)
        {
            DonationCampaign donationCampaign = repository.FindOne(donationCampaign => donationCampaign.Hash == hash);
            
            if(donationCampaign == null)
            {
                logger.LogWarning($"Campaign not found. | Campaign: {hash}.");

                notification.AddNotification(Domain.Enumerators.MappedErrors.CampaigNotFound);

                return;
            }

            if(donationCampaign?.User.Username != user.Username)
            {
                logger.LogWarning($"This campaign does not belong to this user. | User: {user.Username}.");

                notification.AddNotification(Domain.Enumerators.MappedErrors.WrongCampaignUser);

                return;
            }

            if (!donationCampaign.IsActive)
            {
                logger.LogWarning($"This campaign is already inactivated. | Campaign: {hash}.");

                notification.AddNotification(Domain.Enumerators.MappedErrors.CampaginInactive);

                return;
            }

            donationCampaign.UpdateDonationCampaign(donationCampaign.User.Adapt<User>(), donationCampaign.Hash, false,
                donationCampaign.Title, donationCampaign.Description, donationCampaign.Image, donationCampaign.TargetValue);

            // Persist it
            repository.ReplaceOne(donationCampaign);
        }

        public DonationCampaignDTO Get(string hash)
        {
            DonationCampaign donationCampaign = repository.FindOne(donationCampaign => donationCampaign.Hash == hash && donationCampaign.IsActive == true);

            return donationCampaign.Adapt<DonationCampaignDTO>();
        }

        public List<DonationCampaignDTO> ListAllCampaign()
        {
            List<DonationCampaign> donationCampaign = repository.FilterBy(e => e.IsActive == true).ToList();

            List<DonationCampaignDTO> response = new List<DonationCampaignDTO>();

            foreach (DonationCampaign donation in donationCampaign)
            {
                if (donation.User != null)
                    donation.User.MerchantSafe2Pay = null;

                response.Add(donation.Adapt<DonationCampaignDTO>());
            }

            return response;
        }

        public List<DonationCampaignDTO> ListGridByName(string campaignName, string userName)
        {
            List<DonationCampaign> donationCampaign = repository.FilterBy(e => e.Title == campaignName && e.User.Username == userName && e.IsActive == true).ToList();

            List<DonationCampaignDTO> response = new List<DonationCampaignDTO>();

            foreach (DonationCampaign donation in donationCampaign)
            {
                if (donation.User != null)
                    donation.User.MerchantSafe2Pay = null;

                response.Add(donation.Adapt<DonationCampaignDTO>());
            }

            return response;
        }

        public List<DonationCampaignDTO> ListGridByUserName(string userName)
        {
            List<DonationCampaign> donationCampaign = repository.FilterBy(e => e.User.Username == userName && e.IsActive == true).ToList();

            List<DonationCampaignDTO> response = new List<DonationCampaignDTO>();

            foreach (DonationCampaign donation in donationCampaign)
            {
                if (donation.User != null)
                    donation.User.MerchantSafe2Pay = null;

                response.Add(donation.Adapt<DonationCampaignDTO>());
            }

            return response;
        }

        public List<DonationCampaignDTO> ListSite(string campaignName)
        {
            List<DonationCampaign> donationCampaign = repository.FilterBy(e => e.Title == campaignName && e.IsActive == true).ToList();

            List<DonationCampaignDTO> response = new List<DonationCampaignDTO>();

            foreach (DonationCampaign donation in donationCampaign)
            {
                if (donation.User != null)
                    donation.User.MerchantSafe2Pay = null;

                response.Add(donation.Adapt<DonationCampaignDTO>());
            }

            return response;
        }

        public void Update(DonationCampaignDTO campaign, UserDTO user)
        {
            DonationCampaign data = repository.FindOne(data => data.Hash == campaign.Hash);

            if(data.User.Username != user.Username)
            {
                logger.LogWarning("This campaign does not belong to this user.");

                notification.AddNotification(Domain.Enumerators.MappedErrors.WrongCampaignUser);

                return;
            }

            if (data is null)
            {
                logger.LogWarning($"Campaign not found. | Campaign: {campaign.Hash}.");

                notification.AddNotification(Domain.Enumerators.MappedErrors.CampaigNotFound);

                return;
            }

                // Update user data
            if(campaign.Image == "" || campaign.Image == null)
                data.UpdateDonationCampaign(data.User, campaign.Hash, campaign.IsActive,
                campaign.Title, campaign.Description, data.Image, campaign.TargetValue);
            else
                data.UpdateDonationCampaign(data.User, campaign.Hash, campaign.IsActive,
                campaign.Title, campaign.Description, campaign.Image, campaign.TargetValue);


            // Persist it
            repository.ReplaceOne(data);
        }
    }
}
