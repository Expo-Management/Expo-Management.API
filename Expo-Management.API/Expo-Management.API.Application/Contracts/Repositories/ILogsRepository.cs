using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface ILogsRepository
    {
        Task<Response?> GetLogsAsync();
    }
}
