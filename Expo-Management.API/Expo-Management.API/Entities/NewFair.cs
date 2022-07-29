using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class NewFair
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

    }
}
