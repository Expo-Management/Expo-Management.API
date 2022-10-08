
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "La descripción de la categoría debe de ser entre 2 y 20 caracteres.")]
        public string Description { get; set; } = default!;

    }
}
