using Expo_Management.API.Entities.Events;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;

        public EventsController(
            IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> GetEvents()
        {
            var response = await _eventsRepository.GetEventsAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("event")]
        public async Task<IActionResult> GetEvents(int EventId)
        {
            var response = await _eventsRepository.GetEventAsync(EventId);

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("event")]
        public async Task<IActionResult> CreateEvent([FromBody] EventInput model)
        {
            var response = await _eventsRepository.CreateEventAsync(model);

            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok("Evento creado exitosamente!");
            }
        }

        [HttpPut]
        [Route("event")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventUpdate model)
        {
            var response = await _eventsRepository.UpdateEventAsync(model);

            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok("Evento actualizado exitosamente!");
            }
        }

        [HttpDelete]
        [Route("event")]
        public async Task<IActionResult> DeleteEvent(int EventId)
        {
            var response = await _eventsRepository.DeleteEventAsync(EventId);

            if (response)
            {
                return BadRequest();
            }
            else
            {
                return Ok("Evento removido exitosamente");
            }
        }

        [HttpGet]
        [Route("news")]
        public async Task<IActionResult> GetNews(int FairId)
        {
            var response = await _eventsRepository.GetNewsByFairIdAsync(FairId);

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("security-protocols")]
        public async Task<IActionResult> GetSecurityProtocols(int FairId)
        {
            var response = await _eventsRepository.GetGetSecurityProtocols(FairId);

            if (response == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
