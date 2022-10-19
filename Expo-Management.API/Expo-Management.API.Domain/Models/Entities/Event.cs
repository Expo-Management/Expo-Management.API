using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; } = default!;

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Location { get; set; } = default!;

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string? Details { get; set; }

        public bool AllDay { get; set; }

        public KindEvents KindEvents { get; set; } = default!;

        public Fair Fair { get; set; } = default!;

    }
}
