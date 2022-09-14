using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
    
        [Required]
        [StringLength(100)]
        public string Description { get; set; } = default!;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = default!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string? Details { get; set; }

        public Fair Fair { get; set; } = default!;

    }
}
