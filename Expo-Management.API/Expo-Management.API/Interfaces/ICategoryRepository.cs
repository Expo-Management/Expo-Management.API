using Expo_Management.API.Entities;

namespace Expo_Management.API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(NewCategory model);
        Task<bool> DeleteCategoryAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
    }
}
