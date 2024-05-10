using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SchoolManagement.Startup.BuilderExtensions
{
    public class SwaggerExtension : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "ExportStudents" && operation.Responses.ContainsKey("200"))
            {
                operation.Responses["200"].Content.Add("application/octet-stream", new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                });
            }
        }
    }
}
