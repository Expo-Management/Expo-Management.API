﻿using Microsoft.AspNetCore.Identity;

namespace Expo_Management.API.Entities
{
    public class User: IdentityUser
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public FilesModel? ProfilePicture { get; set; }
        public ProjectModel? Project { get; set; }
        public bool IsLead { get; set; }
    }
}
