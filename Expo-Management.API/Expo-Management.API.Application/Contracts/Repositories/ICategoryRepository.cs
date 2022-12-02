using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface ICategoryRepository
    {
        Task<Response> CreateCategoryAsync(NewCategoryInputModel model);
        Task<Response> DeleteCategoryAsync(int id);
        Task<Response> GetAllCategoriesAsync();
        Task<Response> GetCategoryAsync(int id);
    }
}