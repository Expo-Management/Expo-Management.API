using Microsoft.EntityFrameworkCore;
using Expo_Management.API.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expo_Management.API.Application.Contracts.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> User { get; set; }
        DbSet<Fair> Fair { get; set; }
        DbSet<Event> Event { get; set; }
        DbSet<Logs> Logs { get; set; }
        DbSet<New> New { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Mention> Mention { get; set; }
        DbSet<SecurityProtocols> SecurityProtocols { get; set; }
        DbSet<Qualifications> Qualifications { get; set; }
        DbSet<Claim> Claim { get; set; }
        DbSet<Recommendation> JudgeRecommendation { get; set; }
    }
}
