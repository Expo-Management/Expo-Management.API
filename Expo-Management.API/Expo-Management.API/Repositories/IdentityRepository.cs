using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Expo_Management.API.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityRepository> _logger;
        private AuthUtils _authUtils;

        public IdentityRepository(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<IdentityRepository> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;

            _authUtils = new AuthUtils(_userManager, _roleManager, _configuration);
        }

        public async Task<Response> RegisterNewUser(string Role, RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("Error al registrar un usuario, ya existe");
                return new Response { Status = "Error", Message = "User already exists!" };
            }

            User user = new()
            {
                UserId = model.Id,
                Name = model.Name,
                PhoneNumber = model.Phone,
                Lastname = model.Lastname,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al registrar un usuario, contrasena incorrecta");
                return new Response { Status = "Error", Message = "User creation failed! Password needs a special character, a number, an uppercase letter and an lowercase letter." };
            }

            await _authUtils.AssignRole (user, Role);
            _logger.LogCritical("Error al registrar un usuario, contrasena incorrecta");
            return new Response { Status = "Success", Message = "User created successfully!" };
        }

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

                return new LoginResponse {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    Role = userRoleStored
                };
            }
            return null;
        }
    }
}