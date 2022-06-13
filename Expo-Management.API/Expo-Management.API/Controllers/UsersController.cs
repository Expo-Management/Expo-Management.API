using Abp.UI;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _userRepository;

        public UsersController(
            IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
                return Json(response);
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
        public async Task<IActionResult> UpdateJudgeAsync([FromBody] UpdateUser model)
        {
            var response = await _userRepository.UpdateJudgeAsync(model);

            if (response == null)
            {
                return BadRequest("Check username!");
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
            return BadRequest("Judge email does not exists!");
        }

       ////subir foto test
       //[HttpPost]
       //[Route("photo")]
       //public async Task<string> UploadPhotoProfile([FromForm(Name = "uploadedFile")] IFormFile file, string userId)
       //{
       //     var response = await _userRepository.UploadPhotoProfile(file, userId);
       //
       //     if (response == null)
       //     {
       //         return BadRequest("Check photo!");
       //     }
       //     return Ok(response);
       //}

    }
}
