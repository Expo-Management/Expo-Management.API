using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;
using System.Reflection;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IEventsRepository
    {
        Task<Response> GetEventsAsync();
        Task<Event?> GetEventAsync(int EventId);
        Task<Response> CreateEventAsync(EventInputModel Event);
        Task<Response> UpdateEventAsync(EventUpdateInputModel Event);
        Task<bool> DeleteEventAsync(int EventId);

        Task<Response> GetKindEventsAsync();
        Task<Response?> CreateKindEventAsync(newKindEventInputModel model);
        Task<Response?> UpdateKindEventAsync(UpdateKindEventsInputModel model);
        Task<Response> DeleteKindEventAsync(int kindEventId);
        Response GetColorName();

        Task<List<New>?> GetNewsByFairIdAsync(int FairId);
        Task<List<SecurityProtocols>?> GetGetSecurityProtocols(int FairId);
    }
}
