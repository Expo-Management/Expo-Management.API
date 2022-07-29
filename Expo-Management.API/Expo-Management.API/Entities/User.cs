using Expo_Management.API.Entities.Projects;
using Microsoft.AspNetCore.Identity;

namespace Expo_Management.API.Entities
{
    public class User: IdentityUser
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public FilesModel? ProfilePicture { get; set; }
        public ProjectModel? Project { get; set; }
        public bool? IsLead { get; set; }
        public string Position { get; set; }
        public string Institution { get; set; }
    }
}
