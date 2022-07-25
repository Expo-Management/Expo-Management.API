using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _userRepository;

        public UsersController(
            IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Judges

        [HttpGet]
        [Route("judges")]
        public async Task<IActionResult> GetJudgesAsync()
        {
            var response = await _userRepository.GetJudgesAsync();

            if (response == null)
            {
                return NotFound();
            }
            else {
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("judge")]
        public async Task<IActionResult> GetJudgeAsync(string email)
        {
            var response = await _userRepository.GetJudgeAsync(email);

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut]
        [Route("judge")]
        public async Task<IActionResult> UpdateJudgeAsync([FromForm] UpdateUser model)
        {
            var response = await _userRepository.UpdateJudgeAsync(model);

            if (response == null)
            {
                return BadRequest("Hubo un error, por favor, intentelo más tarde!");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("judge")]
        public async Task<IActionResult> DeleteJudgeAsync(string email)
        {
            bool response = await _userRepository.DeleteJudgeAsync(email);

            if (response)
            {
                return Ok("Judge deleted!");
            }
            return BadRequest("Correo incorrecto!");
        }


        //Admins

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

        [HttpPut]
        [Route("admin")]
        public async Task<IActionResult> UpdateAdminAsync([FromBody] UpdateUser model)
        {
            var response = await _userRepository.UpdateAdminAsync(model);

            if (response == null)
            {
                return BadRequest("Correo incorrecto!");
            }
            return Ok(response);
        }

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

        [HttpPut]
        [Route("student")]
        public async Task<IActionResult> UpdateStudentAsync([FromBody] UpdateUser model)
        {
            var response = await _userRepository.UpdateStudentAsync(model);

            if (response == null)
            {
                return BadRequest("Correo incorrecto!");
            }
            return Ok(response);
        }

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
