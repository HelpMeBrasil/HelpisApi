using Donation.API.Models.Management;
using Donation.Application.Contracts;
using Donation.Application.DataTransfer;
using Donation.Infrastructure.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Donation.API.Controllers
{
    [Authorize]
    public class ManagementController : MainController
    {
        private readonly IIdentityProvider provider;
        private readonly IManagementService service;

        public ManagementController(IIdentityProvider provider, IManagementService service)
        {
            this.provider = provider;
            this.service = service;
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ChangePasswordModel model)
        {
            ChangePasswordDTO request = new ChangePasswordDTO(model.Password, model.OldPassword, provider.Username());

            await service.ChangePassword(request);

            return NoContent();
        }
    }
}