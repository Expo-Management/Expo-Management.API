using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(NewCategory model)
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
                return null;
            }
            catch (Exception ex)
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

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Category> GetCategoryAsync(int id)
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
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
