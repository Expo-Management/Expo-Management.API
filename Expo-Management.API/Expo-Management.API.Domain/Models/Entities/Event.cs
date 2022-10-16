using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
    
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = default!;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = default!;

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [StringLength(200)]
        public string? Details { get; set; }

        public bool AllDay { get; set; }

        public KindEvents KindEvents { get; set; } = default!;

        public Fair Fair { get; set; } = default!;

    }
}
