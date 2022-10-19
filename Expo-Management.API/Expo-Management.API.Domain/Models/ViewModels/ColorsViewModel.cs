using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class ColorsViewModel
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string? Value { get; set; } = default!;
    }
}
