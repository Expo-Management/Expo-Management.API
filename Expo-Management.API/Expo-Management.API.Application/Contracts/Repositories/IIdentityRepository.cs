using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;
using Expo_Management.API.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IIdentityRepository
    {
        Task<Response> RegisterNewUser(string Role, RegisterInputModel model);
        Task<LoginResponse?> LoginUser(LoginViewModel model);
        Task<Response> ConfirmEmailAsync(string userId, string token);
        Task<Response> ForgetPasswordAsync(string email);
        Task<Response> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
