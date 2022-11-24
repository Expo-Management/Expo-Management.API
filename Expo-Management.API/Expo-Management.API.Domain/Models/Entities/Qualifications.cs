using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Qualifications
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "El campo solo permite valores entre 1 y 100")]
        public int Punctuation { get; set; }

        public User Judge { get; set; } = default!;

        public Project Project { get; set; } = default!;
    }
}
