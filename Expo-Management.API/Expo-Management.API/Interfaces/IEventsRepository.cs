using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Events;

namespace Expo_Management.API.Interfaces
{
    public interface IEventsRepository
    {
        public Task<List<Event>>? GetEventsAsync();
        public Task<Event>? GetEventAsync(int EventId);
        public Task<Event>? CreateEventAsync(EventInput Event);
        public Task<Event>? UpdateEventAsync(EventUpdate Event);
        public Task<bool> DeleteEventAsync(int EventId);
    }
}
