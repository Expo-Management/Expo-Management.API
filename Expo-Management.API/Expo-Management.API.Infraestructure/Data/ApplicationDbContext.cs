using Expo_Management.API.Application.Contracts.Data;
using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Infraestructure.Data
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Fair> Fair { get; set; } = default!;
        public DbSet<Event> Event { get; set; } = default!;
        public DbSet<Logs> Logs { get; set; } = default!;
        public DbSet<New> New { get; set; } = default!; 
        public DbSet<Files> Files { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Mention> Mention { get; set; } = default!;
        public DbSet<SecurityProtocols> SecurityProtocols { get; set; } = default!;
        public DbSet<Qualifications> Qualifications { get; set; } = default!;
        public DbSet<Claim> Claim { get; set; } = default!;
        public DbSet<Recommendation> JudgeRecommendation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasIndex(u => u.UserId).IsUnique();
        }
    }
}
