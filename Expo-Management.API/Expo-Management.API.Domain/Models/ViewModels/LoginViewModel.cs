using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string? Password { get; set; }
    }
}
