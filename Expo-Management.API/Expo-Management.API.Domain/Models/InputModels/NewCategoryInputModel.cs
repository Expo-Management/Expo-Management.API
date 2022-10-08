using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewCategoryInputModel
    {
        [Required(ErrorMessage = "La descripcion de la categoria es requerida")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "La categoria puede ser maximo de 25 caracteres y un mínimo de 2.")]
        public string Description { get; set; } = default!;
    }
}
