using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de categorias
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] NewCategoryInputModel model)
        {
            var response = await _categoryRepository.CreateCategoryAsync(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener una categoria por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetCategoryAsync(int id)
        {
            var response = await _categoryRepository.GetCategoryAsync(id);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener todas las categorias
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var response = await _categoryRepository.GetAllCategoriesAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para eliminar una categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("category")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var response = await _categoryRepository.DeleteCategoryAsync(id);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }
    }
}