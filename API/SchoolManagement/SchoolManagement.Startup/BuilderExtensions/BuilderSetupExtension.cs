using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Database;
using SchoolManagement.Startup.BuilderExtensions;
using System;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;
public static class BuilderSetupExtension
{
    public static WebApplicationBuilder SetupBuilder(this WebApplicationBuilder builder)
    {
        builder.AddServices();
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c => {
            c.OperationFilter<SwaggerExtension>();
        });
        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        builder.Services.AddDbContext<SchoolManagementDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDatabase")));
        // Cấu hình FileProvider để xử lý các tệp từ thư mục gốc
        //builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

        // Add Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                // ký vào token
                ValidateIssuerSigningKey = true,
                //ValidIssuer = jwtSettings["Issuer"],
                //ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };
        });

        // Add Authorization
        builder.Services.AddAuthorization();
        return builder;
    }
}