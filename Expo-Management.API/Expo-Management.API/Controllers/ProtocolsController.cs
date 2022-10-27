using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de protocolos de seguridad de la feria
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ProtocolsController : Controller
    {
        private readonly IProtocolsRepository _protocolRepository;

        /// <summary>
        /// Constructor del controlador de categorias
        /// </summary>
        /// <param name="protocolRepository"></param>
        public ProtocolsController(IProtocolsRepository protocolRepository)
        {
            _protocolRepository = protocolRepository;
        }

        /// <summary>
        /// Endpoint para crear protocolos de la feria
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("protocol")]
        public async Task<IActionResult> CreateProtocolAsync(string description)
        {
            var response = await _protocolRepository.CreateProtocolAsync(description);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }


        /// <summary>
        /// Endpoint para eliminar un protocolo de seguridad de la feria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("protocol")]
        public async Task<IActionResult> DeleteProtocolAsync(int id)
        {
            var response = await _protocolRepository.DeleteProtocolAsync(id);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });


        }

    }
}


