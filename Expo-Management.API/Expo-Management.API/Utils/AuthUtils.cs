using Expo_Management.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expo_Management.API.Utils
{
    public class AuthUtils
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthUtils(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        private string getRole(string role) {
            if (role.ToLower() == "admin") { return UserRoles.Admin; } 
            else if (role.ToLower() == "judge") { return UserRoles.Judge; }
            else { return UserRoles.User; }

        }

        public async Task AssignRole(User userToAssignRole, string roleToAssign) 
        {
            if (!await _roleManager.RoleExistsAsync(getRole(roleToAssign)))
            {
                await _roleManager.CreateAsync(new IdentityRole(getRole(roleToAssign)));
            }
            await _userManager.AddToRoleAsync(userToAssignRole, getRole(roleToAssign));
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "123456789";
            const string SPECIALS = @"!@£$%^&*()#€";


            public string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
                int passwordSize)
            {
                char[] _password = new char[passwordSize];
                string charSet = ""; // Initialise to blank
                System.Random _random = new Random();
                int counter;

                // Build up the character set to choose from
                if (useLowercase) charSet += LOWER_CASE;

                if (useUppercase) charSet += UPPER_CAES;

                if (useNumbers) charSet += NUMBERS;

                if (useSpecial) charSet += SPECIALS;

                for (counter = 0; counter < passwordSize; counter++)
                {
                    _password[counter] = charSet[_random.Next(charSet.Length - 1)];
                }

                return String.Join(null, _password);
            }
        }
    }
