using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] NewCategory model)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        [HttpDelete]
        [Route("category")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.DeleteCategoryAsync(id);

            if(category)
            {
                return Ok("Categoría eliminada");
            }
            return BadRequest("Hubo un error, por favor, intentelo más tarde.");

        }

    }
}


