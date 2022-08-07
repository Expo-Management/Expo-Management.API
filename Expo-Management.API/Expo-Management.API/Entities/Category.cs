
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } 
    
    }
}
