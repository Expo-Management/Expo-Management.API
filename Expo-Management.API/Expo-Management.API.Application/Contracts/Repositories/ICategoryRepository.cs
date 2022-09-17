using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> CreateCategoryAsync(NewCategoryInputModel model);
        Task<bool> DeleteCategoryAsync(int id);
        Task<List<Category>?> GetAllCategoriesAsync();
        Task<Category?> GetCategoryAsync(int id);
    }
}
