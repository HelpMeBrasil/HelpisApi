using Donation.API.Models.Recovery;
using Donation.Application.Contracts;
using Donation.Application.DataTransfer;
using Donation.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Donation.API.Controllers
{
    [AllowAnonymous]
    public class RecoveryController : MainController
    {
        private readonly IRecoveryService service;
        private readonly ILogger<RecoveryController> logger;

        public RecoveryController(IRecoveryService service, ILogger<RecoveryController> logger)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] PasswordRecoveryRequestModel model)
        {
            logger.LogInformation($"Receiving recovery password request.. | IP: {GetIpAddress()} | User: {model.Username}.");
            
            ChangePasswordDTO request = model.Adapt<ChangePasswordDTO>();

            await service.SendRecoveryCode(request);

            return Ok();
        }
            
        [HttpPost("Confirmation")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Recovery), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm([FromBody] ConfirmationRecoveryModel model)
        {
            logger.LogInformation($"Receiving recovery password confirmation request.. | IP: {GetIpAddress()} | User: {model.Username}.");

            ChangePasswordDTO request = model.Adapt<ChangePasswordDTO>();

            Recovery response = await service.ConfirmRecoveryCode(request);

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] PasswordRecoveryModel model)
        {
            logger.LogInformation($"Receiving recovery password change request.. | IP: {GetIpAddress()} | User: {model.Username} | Recovery Id: {model.RecoveryId}.");

            ChangePasswordDTO request = model.Adapt<ChangePasswordDTO>();

            _ = await service.ChangePassword(request);

            return Ok();
        }
    }
}