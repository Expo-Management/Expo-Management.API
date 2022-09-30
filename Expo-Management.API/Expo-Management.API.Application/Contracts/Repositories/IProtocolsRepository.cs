using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IProtocolsRepository
    {
        Task<SecurityProtocols?> CreateProtocolAsync(string description);
        Task<bool> DeleteProtocolAsync(int id);
    }
}
