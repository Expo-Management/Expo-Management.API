using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de categorias
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Constructor del controlador de categorias
        /// </summary>
        /// <param name="categoryRepository"></param>
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Endpoinr para crear categorias
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] NewCategoryInputModel model)
        {
            try
            {
                var category = await _categoryRepository.CreateCategoryAsync(model);

                if (category == null)
                {
                    return BadRequest("La categoria ya existe.");

                }
                return Ok(category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Endpoint para obtener una categoria por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);

            if (category != null)
            {
                return Ok(category);
            }
            return BadRequest("Hubo un error, por favor, intentelo más tarde.");
        }

        /// <summary>
        /// Endpoint para obtener todas las categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {

            var categories = await _categoryRepository.GetAllCategoriesAsync();

            if (categories != null)
            {
                var domainCategories = new List<Category>();

                foreach (var items in categories)
                {
                    domainCategories.Add(new Category()
                    {
                        Id = items.Id,
                        Description = items.Description
                    });
                }
                return Ok(domainCategories);
            }

            return BadRequest("Hubo un error, por favor, intentelo más tarde.");
        }

        /// <summary>
        /// Endpoint para eliminar una categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("category")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.DeleteCategoryAsync(id);

            if (category)
            {
                return Ok("Categoría eliminada");
            }
            return BadRequest("Hubo un error, por favor, intentelo más tarde.");

        }

    }
}


