using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewRecommendationInputModel
    {
        public int IdProject { get; set; }

        [Required(ErrorMessage = "La recomendacion es requerida es requerida")]
        [StringLength(400, ErrorMessage = "La recomendacion debe ser de un maximo de 400 caracteres.")]
        public string Recommendation { get; set; } = default!;

        [DataType(DataType.EmailAddress)]
        public string correoJuez { get; set; } = default!;

    }
}
