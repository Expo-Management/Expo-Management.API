using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Events;
using Expo_Management.API.Entities.News;
using Expo_Management.API.Interfaces;

namespace Expo_Management.API.Repositories
{
    public class EventsRepository : IEventsRepository
    {

        private readonly ApplicationDbContext _context;

        public EventsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
                    Date = Event.Date,
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

        public async Task<Event>? UpdateEventAsync(EventUpdate Event)
        {
            var result = (from e in _context.Event
                           where e.Id == Event.Id
                           select e).FirstOrDefault();

            if (result != null) 
            {
                result.Description = Event.Description;
                result.Location = Event.Location;
                result.Date = Event.Date;
                result.Details = Event.Details;
                _context.SaveChanges();

                return result;
            }
            return null;

        }

        public async Task<List<New>> GetNewsByFairIdAsync(int FairId) 
        {
            var results = (from n in _context.New
                          where n.Fair.Id == FairId
                          select n).ToList();

            if (results != null && results.Count() > 0)
            {
                return results;
            }
            return null;
        }
    }
}
