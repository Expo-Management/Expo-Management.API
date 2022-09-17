using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewProjectInputModel
    {
        [Required(ErrorMessage = "Nombre de proyecto es requerido")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Descripción del proyecto es requerido")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Correo del lider del proyecto es requerido")]
        public string Lider { get; set; } = default!;

        [Required(ErrorMessage = "Correo del segundo participante del proyecto es requerido")]
        public string Member2 { get; set; } = default!;

        [Required(ErrorMessage = "Correo del tercer participante del proyecto es requerido")]
        public string Member3 { get; set; } = default!;

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public IFormFile Files { get; set; } = default!;

        [Required(ErrorMessage = "Id de la feria del proyecto es requerido")]
        public int Fair { get; set; }

        [Required(ErrorMessage = "Id de la categoría del proyecto es requerido")]
        public int Category { get; set; }

    }
}
