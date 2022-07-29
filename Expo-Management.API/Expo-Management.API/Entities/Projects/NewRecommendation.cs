using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.Projects
{
    public class NewRecommendation
    {
        public int IdProject { get; set; }

        [Required(ErrorMessage = "Descripcion del proyecto es requerida")]
        public string Recommendation { get; set; }
        public string correoJuez { get; set; }

    }
}
