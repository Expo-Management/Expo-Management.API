using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> UpdateStudetProjectAsync(UpdateUserProjectInputModel model);
        Task<String?> GetUserFullName(string email);

        Task<List<User>?> GetJudgesAsync();
        Task<User?> GetJudgeAsync(string email);
        Task<User?> UpdateJudgeAsync(UpdateUserInputModel model);
        Task<bool> DeleteJudgeAsync(string email);


        Task<List<User>?> GetAdminsAsync();
        Task<User?> GetAdminAsync(string email);
        Task<User?> UpdateAdminAsync(UpdateUserInputModel model);
        Task<bool> DeleteAdminAsync(string email);


        Task<List<User>?> GetStudentsAsync();
        Task<User?> GetStudentAsync(string email);
        Task<User?> UpdateStudentAsync(UpdateUserInputModel model);
        Task<bool> DeleteStudentAsync(string email);
    }
}
