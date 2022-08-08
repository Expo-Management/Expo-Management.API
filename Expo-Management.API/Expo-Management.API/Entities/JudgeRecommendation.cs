using System.ComponentModel.DataAnnotations;
using Expo_Management.API.Entities.Projects;

namespace Expo_Management.API.Entities
{
    public class JudgeRecommendation
    {
        public int Id { get; set; }
        public ProjectModel project { get; set; }
        public User user { get; set; }
        [Required(ErrorMessage = "A recomendation is required")]
        public string Recomendacion { get; set; }
    }
}
