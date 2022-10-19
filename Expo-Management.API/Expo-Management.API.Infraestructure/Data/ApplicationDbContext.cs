using Expo_Management.API.Application.Contracts.Data;
using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Emit;

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
        public DbSet<Files> Files { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Mention> Mention { get; set; } = default!;
        public DbSet<SecurityProtocols> SecurityProtocols { get; set; } = default!;
        public DbSet<Qualifications> Qualifications { get; set; } = default!;
        public DbSet<Claim> Claim { get; set; } = default!;
        public DbSet<Recommendation> JudgeRecommendation { get; set; } = default!;
        public DbSet<KindEvents> KindOfEvent { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasIndex(u => u.UserId).IsUnique();

            #region Seed-Roles

            //Roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "e4ac8fa9-2478-4158-9184-3a30371793d4", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "f7104c2d-0318-4412-a366-b329d3225afa" });

            //SuperUser
            builder.Entity<User>().HasData(new User
            {
                Id = "3e32e27b-7564-47bc-a84e-c53ec4bce500",
                UserId = "101110111",
                Name = "Super",
                Lastname = "User",
                ProfilePicture = null,
                IsLead = false,
                Position = "System Admin",
                Institution = "Expo Management",
                RefreshToken = null,
                RefreshTokenExpiryTime = DateTime.MinValue,
                UserName = "super_user",
                NormalizedUserName = "SUPER_USER",
                Email = "superuser@gmail.com",
                NormalizedEmail = "SUPERUSER@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAENOnvFWoDlwlguiTnME6AYS+fsCudlGvY4D/DyjHdZ/3sEZgHzz78dd49FRuQKCp1g==",
                SecurityStamp = "WFPEJN3SJ3EFRD5SO5L4ASFJ25PM36LJ",
                ConcurrencyStamp = "ac37d1f9-c93d-4683-8c93-24c2ae031295",
                PhoneNumber = "00000000",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = "3e32e27b-7564-47bc-a84e-c53ec4bce500", RoleId = "e4ac8fa9-2478-4158-9184-3a30371793d4" });

            #endregion
        }
    }
}
