using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de usuarios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        readonly UserManager<User> _userManager;
        private readonly IUsersRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Constructor del controlador de usuarios
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userRepository"></param>
        /// <param name="logger"></param>
        public UsersController(
            UserManager<User> userManager,
            IUsersRepository userRepository,
            ILogger<UsersController> logger
            )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Endopoint para obtener el nombre completo del usuario
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge,User")]
        [HttpGet]
        [Route("get-user-fullname")]
        public async Task<IActionResult> GetUserFullname(string email)
        {
            var response = await _userRepository.GetUserFullName(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        //Judges

        /// <summary>
        /// Endpoint para obtener los jueces
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge")]
        [HttpGet]
        [Route("judges")]
        public async Task<IActionResult> GetJudgesAsync()
        {
            var response = await _userRepository.GetJudgesAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para obtener un juez por el email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge")]
        [HttpGet]
        [Route("judge")]
        public async Task<IActionResult> GetJudgeAsync(string email)
        {
            var response = await _userRepository.GetJudgeAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para actualizar un juez
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge")]
        [HttpPut]
        [Route("judge")]
        public async Task<IActionResult> UpdateJudgeAsync([FromBody] UpdateUserInputModel model)
        {
            var response = await _userRepository.UpdateJudgeAsync(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para eliminar un juez
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("judge")]
        public async Task<IActionResult> DeleteJudgeAsync(string email)
        {
            var response = await _userRepository.DeleteJudgeAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }


        //Admins

        /// <summary>
        /// Endpoint para obtener los profesores
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admins")]
        public async Task<IActionResult> GetAdminsAsync()
        {
            var response = await _userRepository.GetAdminsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para obtener un profesor por el email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAdminAsync(string email)
        {
            var response = await _userRepository.GetAdminAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para actualizar un profesor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("admin")]
        public async Task<IActionResult> UpdateAdminAsync([FromBody] UpdateUserInputModel model)
        {
            var response = await _userRepository.UpdateAdminAsync(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para eliminar un profesor
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("admin")]
        public async Task<IActionResult> DeleteAdminAsync(string email)
        {
            var response = await _userRepository.DeleteStudentAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }


        //Students

        /// <summary>
        /// Endpoint para obtener los estudiantes
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge")]
        [HttpGet]
        [Route("students")]
        public async Task<IActionResult> GetStudentsAsync()
        {
            var response = await _userRepository.GetStudentsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para obtener un estudiante por el correo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge,User")]
        [HttpGet]
        [Route("student")]
        public async Task<IActionResult> GetStudentAsync(string email)
        {
            var response = await _userRepository.GetStudentAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para actualizar un estudiante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Route("student")]
        public async Task<IActionResult> UpdateStudentAsync([FromBody] UpdateUserInputModel model)
        {
            var response = await _userRepository.UpdateStudentAsync(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para eliminar un estudiante
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("student")]
        public async Task<IActionResult> DeleteStudentAsync(string email)
        {
            var response = await _userRepository.DeleteStudentAsync(email);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }
    }
}
