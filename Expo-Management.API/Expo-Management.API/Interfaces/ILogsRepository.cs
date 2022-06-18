using Expo_Management.API.Entities.Logs;

namespace Expo_Management.API.Interfaces
{
    public interface ILogsRepository
    {
        Task<List<Logs>> GetLogsAsync();
    }
}
