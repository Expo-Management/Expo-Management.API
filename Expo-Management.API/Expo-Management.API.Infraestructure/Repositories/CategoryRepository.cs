using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de Categorias de la feria
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;


        /// <summary>
        /// Constructor del repositorio de Categoria de la feria
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Metodo para crear Categorias de la feria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para eliminar Categoria de la feria
        /// </summary>
        /// <param name="Id"></param>
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

                _logger.LogWarning("Error al eliminar una categoría.");
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener las categorias de la feria
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para obtener una categoria de la feria
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
