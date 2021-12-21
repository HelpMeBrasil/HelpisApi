using Donation.API.Models.User;
using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Donation.Infrastructure.Contract;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donation.API.Controllers
{
    [Authorize]
    public class UserController : MainController
    {
        private readonly IUserService service;
        private readonly IIdentityProvider provider;

        public UserController(IUserService service, IIdentityProvider provider)
        {
            this.service = service;
            this.provider = provider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            UserDTO response = service.Get(provider.Id());

            response.MerchantSafe2Pay = null;

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] UserUpdateModel model)
        {
            model.Id = provider.Id();

            service.Update(model.Adapt<UserDTO>(), model.merchant);

            return Ok();
        }
    }
}