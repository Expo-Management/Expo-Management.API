using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IEventsRepository
    {
        Task<Response> GetEventsAsync();
        Task<Response> GetKindEventsAsync();
        Task<Event?> GetEventAsync(int EventId);
        Task<Response> CreateEventAsync(EventInputModel Event);
        Task<Response> UpdateEventAsync(EventUpdateInputModel Event);
        Task<bool> DeleteEventAsync(int EventId);
        Task<List<New>?> GetNewsByFairIdAsync(int FairId);
        Task<List<SecurityProtocols>?> GetGetSecurityProtocols(int FairId);
    }
}
