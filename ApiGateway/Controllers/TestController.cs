using DataServices.Entities.Human;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiGateway.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class TestController(UserManager<ApiUser> _userManager) : ControllerBase {

        [HttpGet("ip")]
        public IActionResult Get()
        {
            string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            return Ok(ip);
        }

        [HttpGet("ip2")]
        public IActionResult Get2()
        {
            string ip = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            return Ok(ip);
        }

        [HttpGet("GetIdentity")]
        public async Task<IActionResult> GetIdentity()
        {
            var identity = await _userManager.GetUserAsync(User); // Added null-conditional operator to avoid null reference
            return Ok(identity);
        }
    }
}