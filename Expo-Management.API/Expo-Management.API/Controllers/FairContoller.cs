using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FairContoller : ControllerBase
    {
        private readonly IFairRepository _fairRepository;

        public FairContoller(IFairRepository fairRepository)
        {
            _fairRepository = fairRepository;
        }

        [HttpPost]
        [Route("fair")]
        public async Task<IActionResult> AddFair([FromForm] NewFair model)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
