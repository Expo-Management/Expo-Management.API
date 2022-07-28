using Expo_Management.API.Entities.Mentions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expo_Management.API.Entities.Projects
{
    public class ProjectModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del proyecto es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descripcion del proyecto es requerido")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Documento del proyecto es requerido")]
        public FilesModel? Files { get; set; }

        public virtual ICollection<Mention> Mentions { get; set; }

        public Fair Fair { get; set; }
    }
}
