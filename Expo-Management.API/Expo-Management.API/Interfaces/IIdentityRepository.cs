using Expo_Management.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Interfaces
{
    public interface IIdentityRepository
    {
        Task<Response> RegisterNewUser(string Role, RegisterModel model);
        Task<LoginResponse> LoginUser(LoginModel model);
    }
}
