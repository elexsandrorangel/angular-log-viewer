using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerUnauthorizedException : LogViewerException
    {
        public LogViewerUnauthorizedException(ExceptionType exceptionType = ExceptionType.Error) : base(exceptionType)
        {
        }

        public LogViewerUnauthorizedException(string message) : base(message)
        {
        }

        public LogViewerUnauthorizedException(Exception exception) : base(exception)
        {
        }

        public LogViewerUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}