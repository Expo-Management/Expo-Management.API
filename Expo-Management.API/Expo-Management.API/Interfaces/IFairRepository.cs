using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Projects;

namespace Expo_Management.API.Interfaces
{
    public interface IFairRepository
    {
        Task<Fair> CreateFairAsync(NewFair model);
        Task<int> GetCurrentFairIdAsync();
        Task<Fair> DeleteFairAsync();
        Task<List<Fair>> GetAllFairsAsync();
    }
}
