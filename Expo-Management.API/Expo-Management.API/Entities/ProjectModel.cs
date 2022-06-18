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

        [Required(ErrorMessage = "Project descrption is required")]
        public string Description { get; set; }

        public string Lider { get; set; }

        [Required(ErrorMessage = "Member 2 is required")]
        public string Member2 { get; set; }

        [Required(ErrorMessage = "Member 3 is required")]
        public string Member3 { get; set; }

        public FilesModel Files { get; set; }


    }
}
