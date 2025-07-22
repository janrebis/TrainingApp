using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Application.Services;
using TrainingApp.Core.Interfaces.Repositories;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Infrastructure;
using TrainingApp.Infrastructure.Identity;
using TrainingApp.Infrastructure.Repositories;
using AutoMapper;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Application.Mapper.Profiles;
namespace TrainingApp.Api
{
    public static class ConfigureServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllersWithViews(options =>
            {

            });

            services.AddDbContext<ApplicationDbContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
             });


            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddScoped<ITrainerRepository, TrainerRepository>();
            services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<ITrainingPlanService, TrainingPlanService>();
            services.AddControllers();
            services.AddAutoMapper(typeof(TrainerProfile));
            services.AddAutoMapper(typeof(TrainingPlanProfile));
            return services;
        }
    }
}
