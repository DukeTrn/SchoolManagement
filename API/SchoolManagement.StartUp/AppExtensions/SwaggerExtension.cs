using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SchoolManagement.StartUp.AppExtensions
{
    internal static class SwaggerExtension
    {
        public static WebApplication UseSwaggerAndUI(this WebApplication app)
        {
            app.UseSwagger().UseSwaggerUI(c => c.DocExpansion(DocExpansion.None));
            return app;
        }
    }
}
