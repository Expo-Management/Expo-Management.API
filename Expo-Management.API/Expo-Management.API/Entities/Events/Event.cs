using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
    
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(150)]
        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string? Details { get; set; }

        public Fair Fair { get; set; }

    }
}
