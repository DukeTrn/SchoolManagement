using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Service;
using SchoolManagement.Service.Authentication;
using SchoolManagement.Service.Data;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Authentication;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Startup.BuilderExtensions
{
    internal static class ServicesExtension
    {
        internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder) => builder.AddToolServices()
            .AddLogicServices();

        private static WebApplicationBuilder AddLogicServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            return builder;
        }
        private static WebApplicationBuilder AddToolServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IEntityFilterService<>), typeof(EntityFilterService<>));
            builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
            return builder;
        }
    }
}
