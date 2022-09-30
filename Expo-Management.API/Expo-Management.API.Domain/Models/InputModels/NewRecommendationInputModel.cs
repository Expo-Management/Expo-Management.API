using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewRecommendationInputModel
    {
        public int IdProject { get; set; }

        [Required(ErrorMessage = "La recomendacion es requerida es requerida")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "La recomendacion debe ser entre 5 y 250 caracteres.")]
        public string Recommendation { get; set; } = default!;

        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El correo debe de ser entre 5 y 100 caracteres.")]
        public string correoJuez { get; set; } = default!;

    }
}
