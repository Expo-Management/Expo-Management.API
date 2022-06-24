using Expo_Management.API.Entities.Auth;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expo_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IConfiguration _Configuration;
        public AuthenticateController(
            IIdentityRepository identityRepository, 
            IConfiguration configuration)
        {
            _identityRepository = identityRepository;
            _Configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _identityRepository.LoginUser(model);

            if (response == null)
            {
                return Unauthorized();
            }
            else 
            {
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _identityRepository.RegisterNewUser("User", model);

            if (response.Status == "Success")
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var response = await _identityRepository.RegisterNewUser("Admin", model);
            _logger.LogWarning("Error al registrar un usuario");

            try
            {
                if (response.Status == "Success")
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        [HttpPost]
        [Route("register-judge")]
        public async Task<IActionResult> RegisterJudge([FromBody] RegisterModel model)
        {
            var response = await _identityRepository.RegisterNewUser("Judge", model);

            if (response.Status == "Success")
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("confirmEmailToken")]
        public async Task<IActionResult> confirmEmailToken(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("error");
            }
            var result = await _identityRepository.ConfirmEmailAsync(userId, token);

            if (result.Status == "Success")
            {
                return Redirect($"{_Configuration["AppUrl"]}/confirmEmail.html");

            }
            return BadRequest("another error");
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("error");

            var result = await _identityRepository.ForgetPasswordAsync(email);

            if (result.Status == "Success")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _identityRepository.ResetPasswordAsync(model);

                if (result.Status == "Success")
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("some properties are not valid");
        }

    }
}
