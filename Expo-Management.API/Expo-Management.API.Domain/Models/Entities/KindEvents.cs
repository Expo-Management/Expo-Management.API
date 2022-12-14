using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class KindEvents
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Primary { get; set; } = default!;

        [Required]
        public string Secondary { get; set; } = default!;
    }
}
