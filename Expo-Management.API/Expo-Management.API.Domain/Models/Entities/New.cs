using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class New
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = default!;

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = default!;

        [Required]
        public DateTime Date { get; set; }

        public User? Publisher { get; set; }

        public Fair Fair { get; set; } = default!;
    }
}
