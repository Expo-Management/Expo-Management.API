using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de categorias
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de categorias
        /// </summary>
        /// <param name="context"></param>
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo para crear categorias
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para eliminar categorias
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para obtener todas las categorias
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para obtener una categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
