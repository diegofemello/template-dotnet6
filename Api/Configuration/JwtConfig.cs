using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Utils.Configurations;
using Application.Services.Interfaces;
using Application.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Api.Configuration
{
    public static class JwtConfig
    {
        public static void AddJwtTConfig(this IServiceCollection services, IConfiguration configuration)
        {
            TokenConfiguration tokenConfigurations = new();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                    configuration.GetSection("TokenConfigurations")
                )
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);
            services.AddTransient<ITokenService, TokenService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        ResponseEnvelope<object> response = 
                            new(401, "Não autenticado!", "Token ausente ou inválido.");

                        context.Response.StatusCode = response.Status;

                        await context.Response.WriteAsJsonAsync(response);
                    },
                    OnForbidden = async context =>
                    {
                        ResponseEnvelope<object> response = 
                            new(403, "Não autorizado!", "Você não tem permissão para acessar esse método.");

                        context.Response.StatusCode = response.Status;
                        await context.Response.WriteAsJsonAsync(response);
                    }
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());

            });
        }

        public static void UseJwtConfig(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
