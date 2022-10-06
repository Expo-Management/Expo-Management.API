using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del proyecto es requerido")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "El nombre de proyecto debe de ser entre 3 y 70 caracteres.")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Descripcion del proyecto es requerido")]
        [StringLength(400, MinimumLength = 10, ErrorMessage = "El nombre de usuario debe de ser entre 10 y 400 caracteres.")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public Files? Files { get; set; } = default!;

        public virtual ICollection<Mention>? Mentions { get; set; }

        public Fair Fair { get; set; } = default!;

        public Category category { get; set; } = default!;

        public string? oldMembers { get; set; } = default!;
    }
}
