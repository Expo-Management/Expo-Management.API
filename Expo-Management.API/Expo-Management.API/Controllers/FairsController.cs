using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Expo_Management.API.Infraestructure.Repositories;

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
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("fair")]
        public async Task<IActionResult> AddFair()
        {
            try
            {
                var fair = await _fairRepository.CreateFairAsync();

                if (fair == null)
                {
                    return BadRequest("La feria ya existe o la feria no esta en un rango dentro del mismo año.");

                }
                return Ok(fair);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Endpoint para eliminar una feria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("fair")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var fair = await _fairRepository.DeleteFairAsync(id);

            if (fair)
            {
                return Ok("Feria eliminada");
            }
            return BadRequest("Hubo un error, por favor, intentelo más tarde.");

        }

        /// <summary>
        /// Metodo para obtener la feria actual
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User,Judge")]
        [HttpGet]
        [Route("current-fair")]
        public IActionResult GetCurrentFair()
        {
            try
            {
                var currentFair =  _fairRepository.GetCurrentFairId();

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
        /// Metodo para obtener los dias restantes de la feria actual
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("left-fair-days")]
        public async Task<IActionResult> GetLeftDaysFair()
        {
            try
            {
                var currentFair = await _fairRepository.GetLeftDaysAsync();

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
