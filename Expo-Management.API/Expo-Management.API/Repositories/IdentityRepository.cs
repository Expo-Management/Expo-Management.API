using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Auth;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Services;
using Expo_Management.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de IdentityUser
    /// </summary>
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityRepository> _logger;
        private AuthUtils _authUtils;
        private readonly IMailService _mailService;

        /// <summary>
        /// Constructor del repositorio de IdentityUser
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="mailService"></param>
        public IdentityRepository(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<IdentityRepository> logger,
            IConfiguration configuration,
            IMailService mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;

            _authUtils = new AuthUtils(_userManager, _roleManager, _configuration);
            _mailService = mailService;
        }

        /// <summary>
        /// Metodo para registrar usuarios
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response> RegisterNewUser(string Role, RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("Error al registrar un usuario, ya existe");
                return new Response { Status = "Error", Message = "User already exists!" };
            }
            var password = _authUtils.GeneratePassword(true, true, true, true, 10);

            User user = new()
            {
                UserId = model.Id,
                Name = model.Name,
                PhoneNumber = model.Phone,
                Lastname = model.Lastname,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Institution = model.Institution,
                Position = model.Position,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al registrar un usuario, contraseña incorrecta");
                return new Response { Status = "Error", Message = "Creacion de usuario fallida, contraseña ocupa una mayuscula, un caracter especial, un numero y al menos debe ser de más de 8 caracteres de largo" };
            }

            await _authUtils.AssignRole(user, Role);
            _logger.LogCritical("Error al registrar un usuario, contraseña incorrecta");

            /*sending email confirmation*/
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedMailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedMailToken);
            string url = $"{_configuration["AppUrl"]}/api/authenticate/confirmEmailToken?userId={user.Id}&token={validEmailToken}";

            dynamic ConfirmEmailTemplate = new DynamicTemplate();

            await _mailService.SendEmailAsync(user.Email, "d-d4e02abcdd534a81a5cd6e3f581eff0f", ConfirmEmailTemplate = new
            {
                email = user.UserName,
                password = password,
                url = url
            });

            return new Response { Status = "Success", Message = "Usuario creado existosamente!" };
        }

        /// <summary>
        /// Metodo para logear usuarios
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LoginResponse?> LoginUser(LoginModel model)
        {
            var userRoleStored = "";
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                Console.WriteLine(userRoles);

                foreach (var userRole in userRoles)
                {
                    Console.WriteLine(userRole);
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    userRoleStored = userRole;
                }

                var token = _authUtils.GetToken(authClaims);

                return new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    Role = userRoleStored,
                    EmailConfirmed = user.EmailConfirmed,
                    Email = user.Email
                };
            }
            return null;
        }

        /// <summary>
        /// Metodo para confirmar la cuenta por medio de un correo electronico
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Response> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("Error al encontrar usuario");
                return new Response { Status = "Error", Message = "Usuario no encontrado" };
            }

            //decode and validate token
            var decodeToken = WebEncoders.Base64UrlDecode(token);
            var normalToken = Encoding.UTF8.GetString(decodeToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al enviar correo");
                return new Response { Status = "Error", Message = "Correo no encontrado" };
            }
            return new Response { Status = "Success", Message = "Cuenta confirmada" };
        }

        /// <summary>
        /// Metodo para la recuperacion de contraseña
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Error al encontrar usuario");
                return new Response { Status = "Error", Message = "Usuario no encontrado" };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedMailToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedMailToken);
            string url = $"{_configuration["WebUrl"]}/administrator/reset-password?email={email}&token={validToken}";
            dynamic ForgetPasswordTemplate = new DynamicTemplate();

            await _mailService.SendEmailAsync(email, "d-9b96ec3a9bb846dd99b1d3c09903e90c", ForgetPasswordTemplate = new
            {
                username = user.UserName,
                url = url
            });

            return new Response { Status = "Success", Message = "Correo enviado existosamente" };

        }

        /// <summary>
        /// Metodo para resetear la contraseña
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                _logger.LogWarning("Error al encontrar usuario");
                return new Response { Status = "Error", Message = "Usuario no encontrado" };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                _logger.LogWarning("Error durante cambio de contraseñas");
                return new Response { Status = "Error", Message = "Contraseña nueva y Confirmación de contrseña nueva no son iguales" };
            }

            var decodeToken = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodeToken);
            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al cambiar contrsaeña");
                return new Response { Status = "Error", Message = "Hubo un error, por favor intentelo más tarde" };
            }

            return new Response { Status = "Success", Message = "Contraseña cambiada existosamente" };
        }
    }
}