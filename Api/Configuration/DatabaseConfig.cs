using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Infrastructure.Context;

namespace Api.Configuration
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("Default");

            MariaDbServerVersion serverVersion = new(new Version(8, 0, 25));

            services.AddDbContext<ContextBase>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connection, serverVersion)
                );

            services.AddDbContext<HostedContext>(options =>
              options.UseMySql(connection, serverVersion),
                    ServiceLifetime.Singleton
                );
        }
    }
}
