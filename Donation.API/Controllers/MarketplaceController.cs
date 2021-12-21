using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Donation.BLL.Contracts;
using Donation.Infrastructure.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Donation.API.Controllers
{
    [Authorize]
    public class MarketplaceController : MainController
    {
        private readonly ISafe2payService safe2PayService;
        private readonly IUserService service;
        private readonly IIdentityProvider provider;

        public MarketplaceController(ISafe2payService safe2PayService, IIdentityProvider provider, IUserService service)
        {
            this.service = service;
            this.safe2PayService = safe2PayService;
            this.provider = provider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            try
            {
                UserDTO user = service.Get(provider.Id());

                var response = safe2PayService.GetSubAccount(user.Id);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete()
        {
            try
            {
                UserDTO user = service.Get(provider.Id());

                safe2PayService.DeleteSubAccount(user.Id);

                user.Active = false;

                service.Update(user, user.Merchant);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
