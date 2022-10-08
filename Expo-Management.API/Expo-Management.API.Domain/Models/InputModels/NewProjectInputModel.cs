using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewProjectInputModel
    {
        [Required(ErrorMessage = "Nombre de proyecto es requerido")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "El nombre del proyecto debe de ser entre 3 y 70 caracteres.")]
        public string Name { get; set; } = default!;


        [Required(ErrorMessage = "Descripción del proyecto es requerido")]
        [StringLength(400, MinimumLength = 10, ErrorMessage = "La descripcion del proyecto debe de ser entre 10 y 400 caracteres.")]
        public string Description { get; set; } = default!;


        [Required(ErrorMessage = "Correo del lider del proyecto es requerido")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "El correo debe de ser entre 10 y 100 caracteres.")]
        public string Lider { get; set; } = default!;


        [Required(ErrorMessage = "Correo del segundo participante del proyecto es requerido")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "El correo debe de ser entre 10 y 100 caracteres.")]
        public string Member2 { get; set; } = default!;


        [Required(ErrorMessage = "Correo del tercer participante del proyecto es requerido")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "El correo debe de ser entre 10 y 100 caracteres.")]
        public string Member3 { get; set; } = default!;

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public IFormFile Files { get; set; } = default!;

        [Required(ErrorMessage = "Id de la feria del proyecto es requerido")]
        public int Fair { get; set; }

        [Required(ErrorMessage = "Id de la categoría del proyecto es requerido")]
        public int Category { get; set; }

    }
}
