using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Logs;
using Expo_Management.API.Entities.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Auth
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Fair> Fair { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<New> New { get; set; }
        public DbSet<FilesModel> Files { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
    }
}
