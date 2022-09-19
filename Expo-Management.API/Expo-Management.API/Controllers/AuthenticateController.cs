using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.ViewModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador de autenticación
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IConfiguration _Configuration;

        /// <summary>
        /// Constructor del controlador de autenticación
        /// </summary>
        /// <param name="identityRepository"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public AuthenticateController(
            IIdentityRepository identityRepository,
            IConfiguration configuration,
            ILogger<AuthenticateController> logger
            )
        {
            _identityRepository = identityRepository;
            _Configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para logearse en el sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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

        /// <summary>
        /// Endpoint para registrarse en el sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            var response = await _identityRepository.RegisterNewUser("User", model);

            if (response.Status == "Success")
            {
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("La cedula no puede ser repetida.");
                return BadRequest("La cedula no puede ser repetida");
            }
        }

        /// <summary>
        /// Enpoint para registrar un profesor en el sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterInputModel model)
        {
            var response = await _identityRepository.RegisterNewUser("Admin", model);

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

        /// <summary>
        /// Endpoint para registrar un juez en el sistema
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-judge")]
        public async Task<IActionResult> RegisterJudge([FromBody] RegisterInputModel model)
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

        /// <summary>
        /// Endpoint para confirmar el emailToken
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
                return Redirect($"{_Configuration["WebUrl"]}/auth/login");

            }
            return BadRequest("Se acaba de dar un error, por favor intentelo más tarde");
        }

        /// <summary>
        /// Endpoint para solicitar un cambio de contraseña
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint para cambiar la contraseña
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
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
            return BadRequest("algunas propiedades escritas no son validas");
        }
    }
}
