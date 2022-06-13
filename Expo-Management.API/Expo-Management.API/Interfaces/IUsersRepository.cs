using Expo_Management.API.Entities;

namespace Expo_Management.API.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetJudgesAsync();
        Task<User> GetJudgeAsync(string email);
        Task<User> UpdateJudgeAsync(UpdateUser model);
        Task<bool> DeleteJudgeAsync(string email);


        Task<List<User>> GetAdminsAsync();
        Task<User> GetAdminAsync(string email);
        Task<User> UpdateAdminAsync(UpdateUser model);
        Task<bool> DeleteAdminAsync(string email);


        Task<List<User>> GetStudentsAsync();
        Task<User> GetStudentAsync(string email);
        Task<User> UpdateStudentAsync(UpdateUser model);
        Task<bool> DeleteStudentAsync(string email);
    }
}
