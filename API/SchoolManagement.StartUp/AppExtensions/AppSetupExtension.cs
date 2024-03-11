using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SchoolManagement.StartUp.AppExtensions;

namespace Microsoft.Extensions.DependencyInjection;
public static class AppSetupExtension
{
    public static WebApplication SetupApp(this WebApplication app)
    {
        app.UseCors();
        //app.UseStaticFiles(new StaticFileOptions
        //{
        //    //Map virtual folder to physical folder. Used for UseSwaggerUI.InjectStylesheet function.
        //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Swagger")),
        //    RequestPath = $"{SwaggerConstants.PrefixWithStartSlash}/swagger/static"
        //});

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerAndUI();
        }

        app.UseHttpsRedirection();

        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseMiddleware<DebugContextMiddleware>();
        //}

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseMiddleware<TrmsContextMiddleware>();

        app.MapControllers().RequireAuthorization();

        return app;
    }
}
