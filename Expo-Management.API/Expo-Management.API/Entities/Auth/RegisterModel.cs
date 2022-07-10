using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserId es requerido")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Apellidos son requeridos")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefono es requerido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Contraseña es requerido")]
        public string Password { get; set; }
    }
}
