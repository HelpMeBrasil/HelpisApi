using Donation.API.Models.DonationCampaign;
using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Donation.BLL.Contracts;
using Donation.BLL.Transfers;
using Donation.Infrastructure.Contract;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donation.API.Controllers
{
    public class DonationCampaignController : MainController
    {
        private readonly IDonationCampaignService donationCampaign;
        private readonly IIdentityProvider provider;
        private readonly IUserService service;

        public DonationCampaignController(IDonationCampaignService donationCampaign, IIdentityProvider provider, IUserService service)
        {
            this.donationCampaign = donationCampaign;
            this.provider = provider;
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DonationCampaignDTO), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery]string hash)
        {
            DonationCampaignDTO response = donationCampaign.Get(hash);

            return Ok(response);
        }

        [HttpGet]
        [Route("ListSite")]
        [ProducesResponseType(typeof(DonationCampaignDTO), StatusCodes.Status200OK)]
        public IActionResult ListSite([FromQuery] string campaignName)
        {
            var result = donationCampaign.ListSite(campaignName);

            return Ok(result);
        }

        [HttpGet]
        [Route("ListAllCampaign")]
        [ProducesResponseType(typeof(DonationCampaignDTO), StatusCodes.Status200OK)]
        public IActionResult ListAllCampaign()
        {
            var result = donationCampaign.ListAllCampaign();

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("ListGridByName")]
        [ProducesResponseType(typeof(DonationCampaignDTO), StatusCodes.Status200OK)]
        public IActionResult ListGridByName([FromQuery] string campaignName)
        {
            UserDTO user = service.Get(provider.Id());

            var result = donationCampaign.ListGridByName(campaignName, user.Username);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("ListGridByUserName")]
        [ProducesResponseType(typeof(DonationCampaignDTO), StatusCodes.Status200OK)]
        public IActionResult ListGridByUserName()
        {
            UserDTO user = service.Get(provider.Id());

            var result = donationCampaign.ListGridByUserName(user.Username);

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] DonationCampaignUpdateModel model)
        {
            UserDTO user = service.Get(provider.Id());

            donationCampaign.Update(model.Adapt<DonationCampaignDTO>(), user);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] DonationCampaignAddModel model)
        {
            UserDTO user = service.Get(provider.Id());

            var response = donationCampaign.Add(model.Adapt<DonationCampaignDTO>(), user);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete([FromQuery] string hash)
        {
            UserDTO user = service.Get(provider.Id());

            donationCampaign.Delete(hash, user);

            return Ok();
        }
    }
}
