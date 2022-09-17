using Expo_Management.API.Domain.Models.Entities;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface ILogsRepository
    {
        Task<List<Logs>?> GetLogsAsync();
    }
}
