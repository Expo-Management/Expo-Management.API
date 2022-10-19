using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class EventInputModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre del evento es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre de evento debe de ser entre 2 y 50 caracteres.")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "La locación es requerida")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "La locacion del evento debe de ser entre 2 y 150 caracteres.")]
        public string Location { get; set; } = default!;

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "La fecha final es requerida")]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Los detalles del evento son requeridos")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Los detalles del evento deben de ser entre 5 y 250 caracteres.")]
        public string? Details { get; set; }

        public bool AllDay { get; set; }

        public int KindEvent { get; set; }

        public int FairId { get; set; }
    }
}
