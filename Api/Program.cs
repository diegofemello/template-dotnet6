
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using System;
using Api.Configuration;
using Application.Utils.Configurations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    IServiceCollection services = builder.Services;

    services.AddScoped<IActionResultExecutor<ObjectResult>, ResponseEnvelopeResultExecutor>();

    services.AddDatabaseConfig(builder.Configuration, builder.Environment);

    services.AddJwtTConfig(builder.Configuration);

    services.AddCors();

    services.AddControllers()
        .AddNewtonsoftJson(x => x.SerializerSettings
            .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddSwaggerConfig();

    services.AddHttpContextAccessor();

    services.AddDependencyInjectionConfig();

    services.Configure<SmtpConfiguration>
        (builder.Configuration.GetSection("SMTPConfig"));
}



WebApplication app = builder.Build();
{

    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    app.UseSwaggerConfig();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    RewriteOptions option = new();
    option.AddRedirect("^$", "swagger");

    app.UseRewriter(option);

    app.UseJwtConfig();

    app.MapControllers();
}


app.Run();