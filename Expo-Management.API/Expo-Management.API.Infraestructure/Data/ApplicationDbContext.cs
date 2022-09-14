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
        public DbSet<User> User { get; set; }
        public DbSet<Fair> Fair { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<New> New { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Mention> Mention { get; set; }
        public DbSet<SecurityProtocols> SecurityProtocols { get; set; }
        public DbSet<Qualifications> Qualifications { get; set; }
        public DbSet<Claim> Claim { get; set; }
        public DbSet<Recommendation> JudgeRecommendation { get; set; }
    }
}
