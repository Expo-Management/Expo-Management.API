using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Events;
using Expo_Management.API.Entities.News;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de eventos
    /// </summary>
    public class EventsRepository : IEventsRepository
    {

        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de eventos
        /// </summary>
        /// <param name="context"></param>
        public EventsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo para crear eventos
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public async Task<Event>? CreateEventAsync(EventInput Event)
        {
            var fair = (from f in _context.Fair
                        where f.Id == Event.FairId
                        select f).FirstOrDefault();

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
            var result = (from e in _context.Event
                           where e.Id == EventId
                           select e).FirstOrDefault();
            try
            {
                _context.Event.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener un evento
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public async Task<Event>? GetEventAsync(int EventId)
        {
            var results = (from e in _context.Event
                           where e.Id == EventId
                           select e).FirstOrDefault();

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
        public async Task<List<Event>>? GetEventsAsync()
        {
            var results = (from e in _context.Event
                           select e).ToList();

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
        public async Task<Event>? UpdateEventAsync(EventUpdate Event)
        {
            var result = (from e in _context.Event
                           where e.Id == Event.Id
                           select e).FirstOrDefault();

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
            return null;

        }

        /// <summary>
        /// Metodo para obtener las noticias de la feria
        /// </summary>
        /// <param name="FairId"></param>
        /// <returns></returns>
        public async Task<List<New>> GetNewsByFairIdAsync(int FairId) 
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
        public async Task<List<SecurityProtocols>> GetGetSecurityProtocols(int FairId)
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
