using System.ComponentModel.DataAnnotations;
using Expo_Management.API.Domain.Models.Entities;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Recommendation
    {
        public int Id { get; set; }
        public Project project { get; set; } = default!;
        public User user { get; set; } = default!;

        [Required(ErrorMessage = "A recomendation is required")]
        public string Recomendacion { get; set; } = default!;
    }
}
