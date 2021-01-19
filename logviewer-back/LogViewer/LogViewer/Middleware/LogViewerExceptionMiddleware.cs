using LogViewer.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LogViewer.Middleware
{
    public class LogViewerExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogViewerExceptionMiddleware> _logger;

        public LogViewerExceptionMiddleware(RequestDelegate next, ILogger<LogViewerExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string errorMessage = exception.Message;
            ExceptionType exceptionType = ExceptionType.Error;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case LogViewerUnauthorizedException e1:
                    exceptionType = e1.ExceptionType;
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case LogViewerForbiddenException e2:
                    exceptionType = e2.ExceptionType;
                    statusCode = HttpStatusCode.Forbidden;
                    break;

                case LogViewerNotFoundException e3:
                    exceptionType = e3.ExceptionType;
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case LogViewerConflictException cex:
                    exceptionType = cex.ExceptionType;
                    statusCode = HttpStatusCode.Conflict;
                    break;

                case LogViewerUnsupportedMediaTypeException e4:
                    exceptionType = e4.ExceptionType;
                    statusCode = HttpStatusCode.UnsupportedMediaType;
                    break;

                case LogViewerTimeoutException e5:
                    exceptionType = e5.ExceptionType;
                    errorMessage = "Timeout";
                    statusCode = HttpStatusCode.GatewayTimeout;
                    break;

                case LogViewerPreconditionFailedException e6:
                    exceptionType = e6.ExceptionType;
                    statusCode = HttpStatusCode.PreconditionFailed;
                    break;

                case UnprocessableEntityError e7:
                    exceptionType = e7.ExceptionType;
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    break;

                case LogViewerException e:
                    exceptionType = e.ExceptionType;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case NotImplementedException _:
                    errorMessage = "Resource not implemented";
                    statusCode = HttpStatusCode.NotImplemented;
                    break;

                default:
                    errorMessage = "Ops! Tivemos um problema, tente novamente mais tarde!";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            try
            {
                switch (exceptionType)
                {
                    case ExceptionType.Info:
                        //_logger.LogInformation(exception, errorMessage);
                        Log.Information(exception, errorMessage);
                        break;

                    case ExceptionType.Warning:
                        //_logger.LogWarning(exception, errorMessage);
                        Log.Warning(exception, errorMessage);
                        break;

                    case ExceptionType.Error:
                        //_logger.LogError(exception, errorMessage);
                        Log.Error(exception, errorMessage);
                        break;

                    case ExceptionType.Critical:
                        //_logger.LogCritical(exception, errorMessage);
                        Log.Fatal(exception, errorMessage);
                        break;
                }
            }
            catch (Exception)
            {
            }
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                success = false,
                message = errorMessage,
                exception_type = exceptionType
            }));
        }
    }
}
