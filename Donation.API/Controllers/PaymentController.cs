using Donation.BLL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Safe2Pay.Models;
using System;

namespace Donation.API.Controllers
{
    public class PaymentController : MainController
    {
        private readonly ISafe2payService safe2PayService;
        public PaymentController(ISafe2payService safe2PayService)
        {
            this.safe2PayService = safe2PayService;
        }

        [HttpPost]
        [Route("AddTransactionCredit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddTransactionCredit([FromBody] Transaction<CreditCard> transaction, [FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.AddTransactionCredit(transaction, hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("AddTransactionDebit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTransactionDebit([FromBody] Transaction<DebitCard> transaction, [FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.AddTransactionDebit(transaction, hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        [HttpPost]
        [Route("AddTransactionCripto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTransactionCripto([FromBody] Transaction<CryptoCoin> transaction, [FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.AddTransactionCripto(transaction, hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("AddTransactionPix")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTransactionPix([FromBody] Transaction<Safe2Pay.Models.Payment.Pix> transaction, [FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.AddTransactionPix(transaction, hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("AddTransactionBankSlip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTransactionBankSlip([FromBody] Transaction<BankSlip> transaction, [FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.AddTransactionBankSlip(transaction, hash);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("ListPaymentMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListPaymentMethods([FromQuery] string hash)
        {
            try
            {
                var response = safe2PayService.GetPaymentMethods(hash);

                return Ok(response);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
