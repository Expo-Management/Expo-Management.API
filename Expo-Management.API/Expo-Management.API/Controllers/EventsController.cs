﻿using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de eventos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;

        /// <summary>
        /// Constructor del controlador de eventos
        /// </summary>
        /// <param name="eventsRepository"></param>
        public EventsController(
            IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        /// <summary>
        /// Endpoint para obtener los eventos
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para obtener un solo evento
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para crear un evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("event")]
        public async Task<IActionResult> CreateEvent([FromBody] EventInputModel model)
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

        /// <summary>
        /// Endpoint para actualizar un evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("event")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventUpdateInputModel model)
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

        /// <summary>
        /// Endpoint para eliminar un evento
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para obtener las noticias de la feria
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para obtener los protocolos de seguridad de la feria
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
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
