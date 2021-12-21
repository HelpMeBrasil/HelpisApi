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
    public class TransactionController : MainController
    {
        private readonly ISafe2payService safe2PayService;
        private readonly IUserService service;
        private readonly IIdentityProvider provider;
        public TransactionController(ISafe2payService safe2PayService, IIdentityProvider provider, IUserService service)
        {
            this.service = service;
            this.safe2PayService = safe2PayService;
            this.provider = provider;
        }
        [Authorize]
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery] int idTransaction)
        {
            try
            {
                UserDTO user = service.Get(provider.Id());

                var response = safe2PayService.GetTransaction(idTransaction, user.Id);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult List([FromQuery] int rowsPerPage, int pageNumber)
        {
            try
            {
                UserDTO user = service.Get(provider.Id());

                var response = safe2PayService.ListTransaction(user.Id, rowsPerPage, pageNumber);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("ListByReference")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListByReference([FromQuery] string hash)
        {
            try
            {
                decimal response = safe2PayService.ListTransactionByReference(hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
