using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de administrador
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Endpoint para dar de baja el sistema.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("shutdown-system")]
        public async Task<IActionResult> ShutDownSystem()
        {
            return await Task.FromResult(Ok());
        }
    }
}
