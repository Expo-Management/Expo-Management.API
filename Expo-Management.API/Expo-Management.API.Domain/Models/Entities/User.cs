using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class User: IdentityUser
    {
        [Required]
        public string UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Lastname { get; set; } = default!;
        public Files? ProfilePicture { get; set; }
        public Project? Project { get; set; }
        public bool? IsLead { get; set; } = false;
        public string Position { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
