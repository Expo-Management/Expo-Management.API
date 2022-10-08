using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "El nombre de usuario debe de ser entre 5 y 15 caracteres.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string? Password { get; set; }
    }
}
