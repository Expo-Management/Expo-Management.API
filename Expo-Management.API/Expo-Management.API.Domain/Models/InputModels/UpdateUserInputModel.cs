using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class UpdateUserInputModel
    {
        //public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Username es requerido")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Nombre es requerido")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Apellidos son requeridos")]
        public string Lastname { get; set; } = default!;

        [Required(ErrorMessage = "Correo es requerido")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Telefono es requerido")]
        public string Phone { get; set; } = default!;
    }
}
