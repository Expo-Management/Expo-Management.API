using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.News
{
    public class New
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public User? Publisher { get; set; }

        public Fair Fair { get; set; }
    }
}
