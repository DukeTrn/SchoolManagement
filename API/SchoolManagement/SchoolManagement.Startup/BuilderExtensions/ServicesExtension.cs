using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Startup.BuilderExtensions
{
    internal static class ServicesExtension
    {
        internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder) => builder.AddToolServices();
        private static WebApplicationBuilder AddToolServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IEntityFilterService<>), typeof(EntityFilterService<>));
            return builder;
        }
    }
}
