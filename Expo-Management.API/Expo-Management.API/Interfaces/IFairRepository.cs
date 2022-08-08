using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Projects;

namespace Expo_Management.API.Interfaces
{
    public interface IFairRepository
    {
        Task<Fair> CreateFairAsync(NewFair model);
        Task<int> GetCurrentFairIdAsync();
        public Task<bool> DeleteFairAsync(int id);
        Task<List<Fair>> GetAllFairsAsync();
    }
}
