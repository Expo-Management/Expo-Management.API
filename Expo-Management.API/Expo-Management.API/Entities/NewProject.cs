using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class NewProject
    {
        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Project Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Lider is required")]
        public string Lider { get; set; }

        [Required(ErrorMessage = "Member 2 is required")]
        public string Member2 { get; set; }

        [Required(ErrorMessage = "Member 3 is required")]
        public string Member3 { get; set; }

        [Required(ErrorMessage = "Project File is required")]
        public IFormFile Files { get; set; }

        [Required(ErrorMessage = "Project Fair is required")]
        public DateTime Fair { get; set; }

    }
}
