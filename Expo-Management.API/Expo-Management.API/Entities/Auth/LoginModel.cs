using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string? Password { get; set; }
    }
}
