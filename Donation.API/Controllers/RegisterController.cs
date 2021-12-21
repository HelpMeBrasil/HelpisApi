using Donation.API.Models.User;
using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : MainController
    {
        private readonly IUserService service;
        public RegisterController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] UserAddModel model)
        {
            UserDTO request = model.Adapt<UserDTO>();

            service.Add(request);

            return Ok();
        }
    }
}
