using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;

namespace SchoolManagement.Startup.Filters
{
    internal sealed class ExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            string errorMessage = string.Empty;
            var responseContent = new ResponseModel
            {
                Result = false,
            };

            if (context.Exception is LogicException logicException)
            {
                errorMessage = logicException.LogMessage;
                responseContent.Data = logicException.ErrorData;
                responseContent.Message = logicException.Message;
                responseContent.MessageType = logicException.MessageType;
                responseContent.NotifactionType = logicException.NotifactionType;
            }

            if (context.Exception is ExistRecordException existException)
            {
                errorMessage = existException.LogMessage;
                responseContent.Data = existException.ErrorData;
                responseContent.Message = existException.Message;
                responseContent.MessageType = existException.MessageType;
                responseContent.NotifactionType = existException.NotifactionType;
                logger.LogWarning(exception: context.Exception,
                message: "Get exist record exception, TraceParentId:[{trace}]. Message is {msg}",
                responseContent.TraceParentId, errorMessage);
            }
            else
            {
                logger.LogError(exception: context.Exception,
                message: "Get unhandled exception, TraceParentId:[{trace}]. Message is {msg}",
                responseContent.TraceParentId, errorMessage);
            }

            context.Result = new JsonResult(responseContent);

            logger.LogInformation("Response body: {res}", responseContent);
            context.ExceptionHandled = true;

        }
    }
}
