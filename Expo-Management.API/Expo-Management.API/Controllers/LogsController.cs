using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Infraestructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de logs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository _logs;

        /// <summary>
        /// Constructor del controlador de logs
        /// </summary>
        /// <param name="logs"></param>
        public LogsController(ILogsRepository logs)
        {
            _logs = logs;
        }

        /// <summary>
        /// Endpoint para obtener los logs
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var response = await _logs.GetLogsAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
