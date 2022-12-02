using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class QualifyProjectInputModel
    {
        [Required]
        [Range(1, 100, ErrorMessage = "El campo solo permite valores entre 1 y 100")]
        public int Punctuation { get; set; }

        public int ProjectId { get; set; }

        public string JudgeEmail { get; set; } = default!;
    }
}