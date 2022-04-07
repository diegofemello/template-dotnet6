using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repository;
using Infrastructure.Repository.Generic;
using Infrastructure.Repository.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Application.Services.Generic;
using Application.Services.BackgroundServices;
using System;

namespace Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfig(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IGenericService, GenericService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();

            // Background Services
            services.AddSingleton<IHostedRepository, HostedRepository>();
            services.AddCronJob<ExampleHostedService>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"* * * * *";
            });

            // Repositories
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
