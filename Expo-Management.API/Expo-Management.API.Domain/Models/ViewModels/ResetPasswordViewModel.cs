using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; } = default!;

        [Required(ErrorMessage = "Correo es requerido")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "El correo debe de ser entre 10 y 100 caracteres.")]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El contraseña debe de ser entre 5 y 50 caracteres.")]
        public string NewPassword { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El contraseña debe de ser entre 5 y 50 caracteres.")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
