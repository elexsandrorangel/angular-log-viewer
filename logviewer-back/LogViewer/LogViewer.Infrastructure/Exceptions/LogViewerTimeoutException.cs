using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerTimeoutException : LogViewerException
    {
        public LogViewerTimeoutException(ExceptionType exceptionType = ExceptionType.Error) : base(exceptionType)
        {
        }

        public LogViewerTimeoutException(string message) : base(message)
        {
        }

        public LogViewerTimeoutException(Exception exception) : base(exception)
        {
        }

        public LogViewerTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}