using Microsoft.Extensions.Configuration;
using Expo_Management.API.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Expo_Management.API.Application.Contracts.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Infraestructure.Repositories;
using Expo_Management.API.Infraestructure.Services;

namespace Expo_Management.API.Infraestructure
{
    public static class Injection
    {
        public static IServiceCollection RegisterInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")), ServiceLifetime.Transient);

            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();
            services.AddScoped<IFilesUploaderRepository, FilesUploaderRepository>();
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IFairRepository, FairRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProtocolsRepository, ProtocolsRepository>();
            services.AddTransient<IMailService, SenderGridMailService>();

            services.AddScoped<IApplicationDbContext>
                (options => options.GetService<ApplicationDbContext>());

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;

        }
    }
}
