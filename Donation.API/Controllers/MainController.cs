using Microsoft.AspNetCore.Mvc;

namespace Donation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        protected string GetIpAddress() => HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}