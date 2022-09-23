using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IFairRepository
    {
        Task<Fair?> CreateFairAsync(DateTime model);
        Task<int> GetCurrentFairIdAsync();
        Task<int> GetCurrentFairDaysAsync();
        public Task<bool> DeleteFairAsync(int id);
        Task<List<Fair>?> GetAllFairsAsync();
    }
}
