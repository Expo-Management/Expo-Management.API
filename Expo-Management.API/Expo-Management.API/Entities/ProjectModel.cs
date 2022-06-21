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

        [Required(ErrorMessage = "Lider is required")]
        public User Lider { get; set; }

        [Required(ErrorMessage = "Member 2 is required")]
        public User Member2 { get; set; }

        [Required(ErrorMessage = "Member 3 is required")]
        public User Member3 { get; set; }

        [Required(ErrorMessage = "Project File is required")]
        public FilesModel Files { get; set; }

        [Required(ErrorMessage = "Project Fair is required")]
        public Fair Fair { get; set; }

    }
}
