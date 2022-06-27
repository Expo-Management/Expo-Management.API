using Expo_Management.API.Entities.Mentions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expo_Management.API.Entities
{
    public class ProjectModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Project Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Project File is required")]
        public FilesModel? Files { get; set; }

        public virtual ICollection<Mention> Mentions { get; set; }

        public Fair Fair { get; set; }
    }
}
