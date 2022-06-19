using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Events;
using Expo_Management.API.Entities.News;

namespace Expo_Management.API.Interfaces
{
    public interface IEventsRepository
    {
        Task<List<Event>>? GetEventsAsync();
        Task<Event>? GetEventAsync(int EventId);
        Task<Event>? CreateEventAsync(EventInput Event);
        Task<Event>? UpdateEventAsync(EventUpdate Event);
        Task<bool> DeleteEventAsync(int EventId);
        Task<List<New>> GetNewsByFairIdAsync(int FairId);
    }
}
