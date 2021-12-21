using Donation.API.Models.Identity;
using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Donation.API.Controllers
{
    public class AuthController : MainController
    {
        private readonly IIdentityService service;
        private readonly ILogger<AuthController> logger;

        public AuthController(IIdentityService service, ILogger<AuthController> logger)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IdentityDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AuthModel model)
        {
            AuthDTO resquest = model.Adapt<AuthDTO>();

            logger.LogInformation($"Receiving authentication request.. | IP: {GetIpAddress()} | User: {model.Username}.");

            IdentityDTO response = await service.SignInAsync(resquest);

            return Ok(response);
        }
    }
}