using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.Projects
{
    public class NewProject
    {
        [Required(ErrorMessage = "Nombre de proyecto es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descripción del proyecto es requerido")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Correo del lider del proyecto es requerido")]
        public string Lider { get; set; }

        [Required(ErrorMessage = "Correo del segundo participante del proyecto es requerido")]
        public string Member2 { get; set; }

        [Required(ErrorMessage = "Correo del tercer participante del proyecto es requerido")]
        public string Member3 { get; set; }

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public IFormFile Files { get; set; }

        [Required(ErrorMessage = "Id de la feria del proyecto es requerido")]
        public int Fair { get; set; }

        [Required(ErrorMessage = "Id de la categoría del proyecto es requerido")]
        public int Category { get; set; }

    }
}
