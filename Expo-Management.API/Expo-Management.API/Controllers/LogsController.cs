using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository _logs;

        public LogsController(ILogsRepository logs)
        {
            _logs = logs;
        }

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
