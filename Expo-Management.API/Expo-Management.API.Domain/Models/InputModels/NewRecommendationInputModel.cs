using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewRecommendationInputModel
    {
        public int IdProject { get; set; }

        [Required(ErrorMessage = "Descripcion del proyecto es requerida")]
        public string Recommendation { get; set; } = default!;
        public string correoJuez { get; set; } = default!;

    }
}
