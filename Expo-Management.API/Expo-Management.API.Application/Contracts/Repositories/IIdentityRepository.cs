using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;
using Expo_Management.API.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IIdentityRepository
    {
        Task<Response> RegisterNewUser(string Role, RegisterInputModel model);
        Task<LoginResponse?> LoginUser(LoginViewModel model);
        Task<Response> ConfirmEmailAsync(string userId, string token);
        Task<Response> ForgetPasswordAsync(string email, string role);
        Task<Response> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<TokenModel?> RefreshToken(TokenModel tokenModel);
    }
}
