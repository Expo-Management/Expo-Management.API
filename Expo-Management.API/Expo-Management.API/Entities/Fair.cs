using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class Fair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
    }
}
