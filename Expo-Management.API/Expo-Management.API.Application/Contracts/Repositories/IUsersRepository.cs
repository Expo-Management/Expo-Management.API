using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IUsersRepository
    {
        Task<Response?> UpdateStudetProjectAsync(UpdateUserProjectInputModel model);
        Task<Response?> GetUserFullName(string email);

        Task<Response?> GetJudgesAsync();
        Task<Response?> GetJudgeAsync(string email);
        Task<Response?> UpdateJudgeAsync(UpdateJudgeInputModel model);
        Task<Response> DeleteJudgeAsync(string email);

        Task<Response?> GetAdminsAsync();
        Task<Response?> GetAdminAsync(string email);
        Task<Response?> UpdateAdminAsync(UpdateUserInputModel model);
        Task<Response> DeleteAdminAsync(string email);

        Task<Response?> GetStudentsAsync();
        Task<Response?> GetStudentAsync(string email);
        Task<Response?> UpdateStudentAsync(UpdateUserInputModel model);
        Task<Response> DeleteStudentAsync(string email);
    }
}
