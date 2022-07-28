using Expo_Management.API.Entities.Projects;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class UpdateUser
    {
        [Required(ErrorMessage = "UserId es requerido")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username es requerido")]
        public string UserName { get; set; }

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

        [Required(ErrorMessage = "Id del proyecto es requerido")]
        public ProjectModel Project { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
