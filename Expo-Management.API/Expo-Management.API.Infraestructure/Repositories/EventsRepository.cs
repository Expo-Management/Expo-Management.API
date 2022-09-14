using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de eventos
    /// </summary>
    public class EventsRepository : IEventsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventsRepository> _logger;

        /// <summary>
        /// Constructor del repositorio de eventos
        /// </summary>
        /// <param name="context"></param>
        public EventsRepository(ApplicationDbContext context, ILogger<EventsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para crear eventos
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task<Event?> CreateEventAsync(EventInputModel Event)
        {
            var fair = await (from f in _context.Fair
                        where f.Id == Event.FairId
                        select f).FirstOrDefaultAsync();

            if (fair != null) 
            {
                var newEvent = new Event()
                {
                    Description = Event.Description,
                    Location = Event.Location,
                    StartDate = Event.StartDate,
                    EndDate = Event.EndDate,
                    Details = Event.Details,
                    Fair = fair
                };

                if (_context.Event.Add(newEvent) != null)
                {
                    _context.SaveChanges();
                    return newEvent;
                }
                else
                {
                    _logger.LogWarning("Error al crear un evento.");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo para eliminar eventos
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEventAsync(int EventId)
        {
            var result = await (from e in _context.Event
                           where e.Id == EventId
                           select e).FirstOrDefaultAsync();
            try
            {
                if(result != null)
                {
                    _context.Event.Remove(result);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("Error al eliminar un evento.");
                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener un evento
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<Event?> GetEventAsync(int EventId)
        {
            var results = await (from e in _context.Event
                           where e.Id == EventId
                           select e).FirstOrDefaultAsync();

            if (results != null)
            {
                return (Event)results;
            }
            return null;
        }

        /// <summary>
        /// Metodo para obtener eventos
        /// </summary>
        /// <returns></returns>
        public async Task<List<Event>?> GetEventsAsync()
        {
            var results = await (from e in _context.Event
                           select e).ToListAsync();

            if (results.Count() > 0)
            {
                return (List<Event>)results;
            }
            return null;
        }

        /// <summary>
        /// Metodo para actualizar eventos
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task<Event?> UpdateEventAsync(EventUpdateInputModel Event)
        {
            var result = await (from e in _context.Event
                           where e.Id == Event.Id
                           select e).FirstOrDefaultAsync();

            if (result != null) 
            {
                result.Description = Event.Description;
                result.Location = Event.Location;
                result.StartDate = Event.StartDate;
                result.EndDate = Event.EndDate;
                result.Details = Event.Details;
                _context.SaveChanges();

                return result;
            }
            _logger.LogWarning("Error al actualizar un evento.");
            return null;

        }

        /// <summary>
        /// Metodo para obtener las noticias de la feria
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
        public async Task<List<New>?> GetNewsByFairIdAsync(int FairId) 
        {
            var results = await (from n in _context.New
                          where n.Fair.Id == FairId
                          select n).Include(n => n.Publisher).ToListAsync();

            if (results != null && results.Count() > 0)
            {
                return results;
            }
            return null;
        }

        /// <summary>
        /// Metodo para obtener los protocolos de seguridad
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
        public async Task<List<SecurityProtocols>?> GetGetSecurityProtocols(int FairId)
        {
            var results = await (from sp in _context.SecurityProtocols
                                 where sp.Fair.Id == FairId
                                 select sp).ToListAsync();

            if (results != null && results.Count() > 0)
            {
                return results;
            }
            return null;
        }
    }
}
