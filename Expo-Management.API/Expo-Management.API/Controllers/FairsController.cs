using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de ferias
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FairsController : ControllerBase
    {
        private readonly IFairRepository _fairRepository;

        /// <summary>
        /// Constructor del controlador de ferias
        /// </summary>
        /// <param name="fairRepository"></param>
        public FairsController(IFairRepository fairRepository)
        {
            _fairRepository = fairRepository;
        }

        /// <summary>
        /// Metodo para añadir una feria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("fair")]
        public async Task<IActionResult> AddFair([FromBody] NewFairInputModel model)
        {
            try
            {
                var fair = await _fairRepository.CreateFairAsync(model);

                if (fair == null)
                {
                    return BadRequest("La feria ya existe.");

                }
                return Ok(fair);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para obetener la feria actual
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User,Judge")]
        [HttpGet]
        [Route("current-fair")]
        public async Task<IActionResult> GetCurrentFair()
        {
            try
            {
                var currentFair = await _fairRepository.GetCurrentFairIdAsync();

                if (currentFair != 0)
                {
                    return Ok(currentFair);
                }
                return Ok(0);
            }
            catch (Exception)
            {
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
                throw;
            }
        }

        /// <summary>
        /// Metodo para obtener todas las ferias
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("fairs")]
        public async Task<IActionResult> ShowFairs()
        {
            try
            {
                var projects = await _fairRepository.GetAllFairsAsync();

                if (projects != null)
                {
                    var domainFair = new List<Fair>();

                    foreach (var items in projects)
                    {
                        domainFair.Add(new Fair()
                        {
                           Id = items.Id,
                           StartDate = items.StartDate,
                           EndDate = items.EndDate,
                           Description = items.Description

                        });
                    }
                    return Ok(domainFair);
                }
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
