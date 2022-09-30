using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class UpdateUserInputModel
    {
        [Range(0, long.MaxValue, ErrorMessage = "La cedula no debe tener letras")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La cedula debe ser de de 9 caracteres.")]
        public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Username es requerido")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "El nombre de usuario debe de ser entre 5 y 15 caracteres.")]
        public string UserName { get; set; } = default!;


        [Required(ErrorMessage = "Nombre es requerido")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe de ser entre 3 y 30 caracteres.")]
        public string Name { get; set; } = default!;


        [Required(ErrorMessage = "Apellidos son requeridos")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Los apellidos deben de ser entre 10 y 100 caracteres.")]
        public string Lastname { get; set; } = default!;


        [Required(ErrorMessage = "Correo es requerido")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El correo debe de ser entre 5 y 100 caracteres.")]
        public string Email { get; set; } = default!;


        [Required(ErrorMessage = "Telefono es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "No es un número valido")]
        [Range(0, long.MaxValue, ErrorMessage = "El telefono no debe tener letras")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El telefono debe ser minumo de 8 números")]
        public string Phone { get; set; } = default!;

    }
}
