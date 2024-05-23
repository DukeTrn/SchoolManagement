﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppSetupExtension
{
    public static WebApplication SetupApp(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("./v1/swagger.json", "School Management V1"); //originally "./swagger/v1/swagger.json"
            //});
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("AllowAllHeaders");
        //app.UseStaticFiles(new StaticFileOptions
        //{
        //    //Map virtual folder to physical folder. Used for UseSwaggerUI.InjectStylesheet function.
        //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Swagger")),
        //    //RequestPath = $"{SwaggerConstants.PrefixWithStartSlash}/swagger/static"
        //});
        return app;
    }
}
