using Expo_Management.API.Entities;

namespace Expo_Management.API.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetJudgesAsync();
        Task<User> GetJudgeAsync(string email);
        Task<User> UpdateJudgeAsync(UpdateUser model);
        Task<bool> DeleteJudgeAsync(string email);
    }
}
