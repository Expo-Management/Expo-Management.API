using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class UpdateUser
    {
        [Required(ErrorMessage = "UserID is required")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
