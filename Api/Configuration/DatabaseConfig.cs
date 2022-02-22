using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Infrastructure.Context;

namespace Api.Configuration
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IWebHostEnvironment env)
        {
            string cnx = env.IsDevelopment() ? "Default" : "Prod";

            string connection = configuration.GetConnectionString(cnx);

            MariaDbServerVersion serverVersion = new(new Version(8, 0, 25));

            services.AddDbContext<ContextBase>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connection, serverVersion)
                );
        }
    }
}
