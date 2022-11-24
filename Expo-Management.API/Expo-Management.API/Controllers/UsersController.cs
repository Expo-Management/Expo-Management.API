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
    public class UsersController : ControllerBase
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
        [Authorize(Roles="Admin,Judge,User")]
        [HttpGet]
        [Route("get-user-fullname")]
        public async Task<IActionResult> GetUserFullname(string email)
        {
            var response = await _userRepository.GetUserFullName(email);

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        //Judges

        /// <summary>
        /// Endpoint para obtener los jueces
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin,Judge")]
        [HttpGet]
        [Route("judges")]
        public async Task<IActionResult> GetJudgesAsync()
        {
            var response = await _userRepository.GetJudgesAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }

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

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Endpoint para actualizar un juez
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Judge")]
        [HttpPut]
        [Route("judge")]
        public async Task<IActionResult> UpdateJudgeAsync([FromBody] UpdateJudgeInputModel model)
        {
            var response = await _userRepository.UpdateJudgeAsync(model);

            if (response == null)
            {
                return BadRequest("Hubo un error, por favor, intentelo más tarde!");
            }
            else
            {
                return Ok(response);
            }

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
            bool response = await _userRepository.DeleteJudgeAsync(email);

            if (response)
            {
                return Ok("Judge deleted!");
            }
            else
            {
                return BadRequest("Correo incorrecto!");
            }

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

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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

            if (response == null)
            {
                return BadRequest("Correo incorrecto!");
            }
            return Ok(response);
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
            bool response = await _userRepository.DeleteStudentAsync(email);

            if (response)
            {
                return Ok("Admin deleted!");
            }
            return BadRequest("Correo incorrecto!");
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

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
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

            if (response == null)
            {
                return BadRequest("Correo incorrecto!");
            }
            return Ok(response);
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
            bool response = await _userRepository.DeleteStudentAsync(email);

            if (response)
            {
                return Ok("Student deleted!");
            }
            return BadRequest("Correo incorrecto!");
        }
    }
}
