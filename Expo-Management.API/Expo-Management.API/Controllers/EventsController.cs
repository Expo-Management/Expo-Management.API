﻿using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de eventos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
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
        [Authorize(Roles = "Admin,Judge,User")]
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
        [Authorize(Roles = "Admin,Judge,User")]
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
        /// Endpoint para obtener los tipos de evento
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge,User")]
        [HttpGet]
        [Route("kind-events")]
        public async Task<IActionResult> GetKindEvents()
        {
            var response = await _eventsRepository.GetKindEventsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error});
        }

        /// <summary>
        /// Endpoint para crear un evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("event")]
        public async Task<IActionResult> CreateEvent([FromBody] EventInputModel model)
        {
            var response = await _eventsRepository.CreateEventAsync(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para actualizar un evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "User,Admin")]
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
        [Authorize(Roles = "Admin,User")]
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
