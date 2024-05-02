using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManagement.Model;
using Microsoft.Extensions.Hosting;

namespace SchoolManagement.Startup.Filters
{
    public sealed class ResponseFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly ILogger<ResponseFilter> logger;
        private readonly IWebHostEnvironment hostEnvironment;
        public ResponseFilter(ILogger<ResponseFilter> logger, IWebHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            var responseModel = new ResponseModel
            {
                Result = true
            };
            if (context.Result is ObjectResult objectResult)
            {
                responseModel.Data = objectResult.Value;
                if (hostEnvironment.IsDevelopment())
                {
                    logger.LogInformation("Response Body: {json}", responseModel);
                }
                else
                {
                    logger.LogInformation("Response successfully. TraceParentId: {traceId}", responseModel.TraceParentId);
                }
                context.Result = new JsonResult(responseModel);
            }
            else if (context.Result is EmptyResult)
            {
                if (hostEnvironment.IsDevelopment())
                {
                    logger.LogInformation("Empty response");
                }
                context.Result = new JsonResult(responseModel);
            }
            else if (context.Result is FileStreamResult fileStreamResult)
            {
                logger.LogInformation("download file name: {name}, size: {size} byte",
                    fileStreamResult?.FileDownloadName, fileStreamResult?.FileStream.Length);

            }
            else if (context.Result is FileContentResult fileContentResult)
            {
                logger.LogInformation("download file name: {name}, size: {size} byte",
                    fileContentResult?.FileDownloadName, fileContentResult?.FileContents.Length);
            }
            base.OnActionExecuted(context);
        }
    }
}
