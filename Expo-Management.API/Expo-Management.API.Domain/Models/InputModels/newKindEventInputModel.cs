using Expo_Management.API.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class newKindEventInputModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Primary { get; set; } = default!;

        [Required]
        public string Secondary { get; set; } = default!;
    }
}
