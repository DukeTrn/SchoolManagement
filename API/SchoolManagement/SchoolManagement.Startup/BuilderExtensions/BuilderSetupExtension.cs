using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SchoolManagement.Common.Enum;
using SchoolManagement.Database;
using SchoolManagement.Startup.BuilderExtensions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;
public static class BuilderSetupExtension
{
    public static WebApplicationBuilder SetupBuilder(this WebApplicationBuilder builder)
    {
        builder.AddServices();
        // Add services to the container.
        //builder.Services.AddControllersWithViews();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            //c.OperationFilter<SwaggerExtension>();
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml"));

            c.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00"),
                Pattern = @"^(\d+\.(\d{1,7})|(\d{1,7}))?((?<=\d)s)?$",
                Description = "Time span formatted as HH:mm:ss",
            });

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management", Version = "v1" });
            //c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        //builder.Services.AddCors(opt =>
        //{
        //    opt.AddDefaultPolicy(builder =>
        //    {
        //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        //    });
        //});
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllHeaders",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKeys"]))
            };
        //{
        //    ValidateIssuer = true,
        //    ValidateAudience = true,
        //    ValidateLifetime = true,
        //    ValidateIssuerSigningKey = true,
        //    ValidIssuer = jwtSettings["Issuer"],
        //    ValidAudience = jwtSettings["Audience"],
        //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKeys"]))
        //};
    });

        // Add Authorization
        // Configure Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleType.Admin.ToString(), policy => policy.RequireRole(RoleType.Admin.ToString()));
            options.AddPolicy(RoleType.HomeroomTeacher.ToString(), policy => policy.RequireRole(RoleType.HomeroomTeacher.ToString()));
            options.AddPolicy(RoleType.Teacher.ToString(), policy => policy.RequireRole(RoleType.Teacher.ToString()));
            options.AddPolicy(RoleType.Student.ToString(), policy => policy.RequireRole(RoleType.Student.ToString()));
        });
        return builder;
    }
}