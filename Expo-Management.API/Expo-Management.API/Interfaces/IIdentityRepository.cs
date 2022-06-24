using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Interfaces
{
    public interface IIdentityRepository
    {
        Task<Response> RegisterNewUser(string Role, RegisterModel model);
        Task<LoginResponse> LoginUser(LoginModel model);
        Task<Response> ConfirmEmailAsync(string userId, string token);
        Task<Response> ForgetPasswordAsync(string email);
        Task<Response> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
