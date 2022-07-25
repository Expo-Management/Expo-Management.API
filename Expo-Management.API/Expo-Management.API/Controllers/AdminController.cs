using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        [Route("shutdown-system")]
        public async Task<IActionResult> ShutDownSystem()
        {
            return Ok();
        }
    }
}
