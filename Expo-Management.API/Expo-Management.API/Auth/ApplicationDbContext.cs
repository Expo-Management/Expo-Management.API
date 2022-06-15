using Expo_Management.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Auth
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }

        public DbSet<FilesModel> Files { get; set; }

        public DbSet<ProjectModel> Projects { get; set; }

    }
}