using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del proyecto es requerido")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Descripcion del proyecto es requerido")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public Files? Files { get; set; } = default!;

        public virtual ICollection<Mention>? Mentions { get; set; }

        public Fair Fair { get; set; } = default!;

        public Category category { get; set; } = default!;

        public string? oldMembers { get; set; } = default!;
    }
}
