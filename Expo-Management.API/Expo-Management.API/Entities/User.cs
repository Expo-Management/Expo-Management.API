using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class User: IdentityUser
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public FilesModel? ProfilePicture { get; set; }
    }
}
