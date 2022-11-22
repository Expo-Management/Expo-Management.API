using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IProtocolsRepository
    {
        Task<Response?> CreateProtocolAsync(string description);
        Task<Response> DeleteProtocolAsync(int id);
    }
}
