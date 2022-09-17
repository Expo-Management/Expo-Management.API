using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category?> CreateCategoryAsync(NewCategoryInputModel model)
        {
            try
            {
                if (model != null)
                {
                    
                    var newCategory = new Category()
                    {

                        Description = model.Description,
                    };

                    if (await _context.Categories.AddAsync(newCategory) != null)
                    {
                        await _context.SaveChangesAsync();
                        return newCategory;
                    }
                }
                _logger.LogWarning("Error al crear una categoría.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Categories
                                    where x.Id == id
                                    select x).FirstOrDefaultAsync();

                if(result != null)
                {
                    _context.Categories.Remove(result);
                    _context.SaveChanges();
                    return true;
                }

                _logger.LogWarning("Error al eliminar una categoría.");
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Category>?> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Categories
                                   where x.Id == id
                                   select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
