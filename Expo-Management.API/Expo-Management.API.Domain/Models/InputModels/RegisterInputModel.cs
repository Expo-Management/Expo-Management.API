using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "UserId es requerido")]
        public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Username es requerido")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Nombre es requerido")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Apellidos son requeridos")]
        public string Lastname { get; set; } = default!;

        [Required(ErrorMessage = "Correo es requerido")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "checkemail", controller: "auth", ErrorMessage = "Email already exist")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Telefono es requerido")]
        public string Phone { get; set; } = default!;
        public string Position { get; set; } = default!;
        public string Institution { get; set; } = default!;
    }
}
