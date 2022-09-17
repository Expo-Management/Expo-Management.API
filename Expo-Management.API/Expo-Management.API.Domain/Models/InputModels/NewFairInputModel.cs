using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewFairInputModel
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

    }
}
