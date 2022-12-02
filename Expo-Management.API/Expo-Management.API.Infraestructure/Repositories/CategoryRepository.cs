using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Expo_Management.API.Domain.Models.Reponses;
using Microsoft.AspNetCore.Http;
using Castle.Windsor.Installer;

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
        public async Task<Response> CreateCategoryAsync(NewCategoryInputModel model)
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
                        return new Response()
                        {
                            Status = 200,
                            Data = newCategory,
                            Message = "Categoría creada exitosamente!"
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Error al crear una categoria.");
                        return new Response()
                        {
                            Status = 500,
                            Message = "No se pudo crear la categoria, intentelo mas tarde."
                        };
                    }
                }
                else
                {
                    return new Response()
                    {
                        Status = 400,
                        Message = "Revise los datos enviados"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar una categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response> DeleteCategoryAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Categories
                                    where x.Id == id
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    _context.Categories.Remove(result);
                    _context.SaveChanges();

                    return new Response()
                    {
                        Status = 202,
                        Message = "La categoria se eliminó correctamente"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 400,
                        Message = "No se pudo eliminar la categoria"
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener las categorias de la feria
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await (from c in _context.Categories
                                        select c).ToListAsync();

                if (categories.Any())
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = categories
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "No hay categorias registradas."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener una categoria de la feria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response> GetCategoryAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Categories
                                    where x.Id == id
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = result
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 404,
                        Message = "No se encontro la cateogoría."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud."
                };
            }
        }
    }
}