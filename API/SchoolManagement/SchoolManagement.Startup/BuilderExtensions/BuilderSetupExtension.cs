using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagement.Database;
using System;

namespace Microsoft.Extensions.DependencyInjection;
public static class BuilderSetupExtension
{
    public static WebApplicationBuilder SetupBuilder(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        builder.Services.AddDbContext<SchoolManagementDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDatabase")));
        return builder;
    }
}