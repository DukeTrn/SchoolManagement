using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Model;
using SchoolManagement.Service;
using SchoolManagement.Service.Authentication;
using SchoolManagement.Service.Data;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Authentication;
using SchoolManagement.Service.Intention.Data;
using SchoolManagement.Service.Intention.ResetPassword;
using SchoolManagement.Service.ResetPassword;

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
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            return builder;
        }
        private static WebApplicationBuilder AddToolServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IEntityFilterService<>), typeof(EntityFilterService<>));
            builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
            // Add email configuration
            var configuration = builder.Configuration;
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            return builder;
        }
    }
}
