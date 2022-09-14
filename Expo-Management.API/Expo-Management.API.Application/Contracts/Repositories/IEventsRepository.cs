using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IEventsRepository
    {
        Task<List<Event>?> GetEventsAsync();
        Task<Event?> GetEventAsync(int EventId);
        Task<Event?> CreateEventAsync(EventInputModel Event);
        Task<Event?> UpdateEventAsync(EventUpdateInputModel Event);
        Task<bool> DeleteEventAsync(int EventId);
        Task<List<New>?> GetNewsByFairIdAsync(int FairId);
        Task<List<SecurityProtocols>?> GetGetSecurityProtocols(int FairId);
    }
}
