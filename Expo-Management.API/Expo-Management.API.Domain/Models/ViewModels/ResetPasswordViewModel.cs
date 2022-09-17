using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = default!;
    }
}
